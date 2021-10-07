import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";

@Injectable({
    providedIn: 'root'
})
export class AuthStorage {

    private readonly ACCESS_TOKEN = 'ACCESS_TOKEN';
    private readonly REFRESH_TOKEN = 'REFRESH_TOKEN';
    private jwtHelper = new JwtHelperService();

    constructor() { }

    public setNewTokens(accessToken: string, refreshToken: string) {
        this.setTokens(accessToken, refreshToken);
    }

    public removeCurrentTokens() {
        this.removeTokens();
    }

    public getAccessToken() {
        return localStorage.getItem(this.ACCESS_TOKEN);
    }

    public getRefreshToken() {
        return localStorage.getItem(this.REFRESH_TOKEN);
    }

    public async getUserId(): Promise<string> {
        var accessToken = this.getAccessToken()
        return this.jwtHelper.decodeToken(accessToken!).Id;
    }

    private setTokens(accessToken: string, refreshToken: string) {
        localStorage.setItem(this.ACCESS_TOKEN, accessToken);
        localStorage.setItem(this.REFRESH_TOKEN, refreshToken);
    }

    private removeTokens() {
        localStorage.removeItem(this.ACCESS_TOKEN);
        localStorage.removeItem(this.REFRESH_TOKEN);
    }
}