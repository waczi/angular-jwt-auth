import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { Observable, throwError } from 'rxjs';
import { AuthStorage } from './auth.storage';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { from } from 'rxjs'
import { RefreshTokenExpiredError } from './auth.token-expired.error';
import { BackendApi } from '../http/backend.api';
import { ReSignInRequestBody } from 'src/app/dto/re-sign-in.request.body';
import { RequestStatus } from 'src/app/dto/request-status';
import { ErrorStatus } from 'src/app/dto/backend-error.response.body';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(
        private authStorage: AuthStorage,
        private backendApi: BackendApi) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.authStorage.getAccessToken()!
        if (token) {
            request = this.appendToken(request, token);
        }

        return next.handle(request).pipe(catchError(error => {
            if (error.status === 401) {
                return from(this.handle401Error(request, next));
            }
            else {
                return throwError(error);
            }
        }));
    }

    private async handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);
            const response = await this.backendApi.reSignIn(await this.getReSignInBody());
            if (response.requestStatus === RequestStatus.Success) {
                this.isRefreshing = false;
                this.authStorage.setNewTokens(response.body.token, response.body.refreshToken)
                this.refreshTokenSubject.next(response.body.token);
                return next.handle(this.appendToken(request, response.body.token)).toPromise();;
            }
            else {
                this.isRefreshing = false;
                if(response.body.status == ErrorStatus.RefreshTokenExpired){
                    this.authStorage.removeCurrentTokens()
                    this.refreshTokenSubject.error("Refresh token expired.");
                    throw new RefreshTokenExpiredError();
                }
                else {
                    this.refreshTokenSubject.error("Re sign in operation failed.");
                    throw Error("Re sign in operation failed with status " + response.body.status)
                }
            }
        }
        else {
            return this.refreshTokenSubject.pipe(
                filter(token => token != null),
                take(1),
                switchMap(jwt => {
                    return next.handle(this.appendToken(request, jwt));
                })).toPromise();
        }
    }

    private async getReSignInBody(): Promise<ReSignInRequestBody> {
        var reSignInBody = new ReSignInRequestBody()
        reSignInBody.refreshToken = this.authStorage.getRefreshToken()!
        reSignInBody.userId = await this.authStorage.getUserId()
        return reSignInBody
    }

    private appendToken(request: HttpRequest<any>, token: string) {
        return request.clone({
            setHeaders: {
                'Authorization': `Bearer ${token}`
            }
        });
    }
}

