import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { SignInRequestBody } from "../../dto/sign-in.request.body";
import { SignInResponseBody } from "../../dto/sign-in.response.body";
import { ReSignInResponseBody } from "../../dto/re-sign-in.response.body";
import { ReSignInRequestBody } from "../../dto/re-sign-in.request.body";
import { ApiResponse } from "../../dto/api.response";
import { RequestStatus } from "../../dto/request-status";
import { CustomerListResponseBody } from "../../dto/customer-list.response.body";

@Injectable({
    providedIn: 'root'
})
export class BackendApi {

    private baseUrl: string = "http://localhost:8080/api"

    constructor(private httpClient: HttpClient) { }

    public async signIn(body: SignInRequestBody) {
        return this.httpClient.post<SignInResponseBody>(
            this.baseUrl + "/auth/signin",
            JSON.stringify(body),
            {
                headers: this.getHttpHeaders()
            }).toPromise().then(this.HandleSuccess).catch(this.HandleError);;
    }

    public async reSignIn(body: ReSignInRequestBody) {
        return this.httpClient.post<ReSignInResponseBody>(
            this.baseUrl + "/auth/resignin",
            JSON.stringify(body),
            {
                headers: this.getHttpHeaders()
            }).toPromise().then(this.HandleSuccess).catch(this.HandleError);;
    }

    public async signOut() {
        return this.httpClient.post<ReSignInResponseBody>(
            this.baseUrl + "/auth/signout",
            {},
            {
                headers: this.getHttpHeaders()
            }).toPromise().then(this.HandleSuccess).catch(this.HandleError);
    }

    public async getCustomerList() {
        return this.httpClient.get<CustomerListResponseBody>(
            this.baseUrl + "/customers/getall",
            {
                headers: this.getHttpHeaders()
            }).toPromise().then(this.HandleSuccess).catch(this.HandleError);
    }

    private HandleSuccess(body: any) {
        return new ApiResponse(
            RequestStatus.Success,
            body)
    }

    private HandleError(error: any): ApiResponse {
        if (error instanceof HttpErrorResponse && error.status !== 0) {
            return new ApiResponse(
                RequestStatus.Failure,
                error.error)
        }
        else {
            throw error;
        }
    }

    private getHttpHeaders() {
        return new HttpHeaders({
            Accept: 'application/json',
            'Content-Type': 'application/json'
        });
    }
}


