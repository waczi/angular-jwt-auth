import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthStorage } from './auth.storage';

@Injectable({
    providedIn: 'root'
})
export class AuthRequiredGuard implements CanActivate {

    constructor(
        private authStorage: AuthStorage,
        private router: Router) { }

    async canActivate(): Promise<boolean> {
        const token = this.authStorage.getAccessToken();
        if (token) {
            return true;
        }
        else {
            this.router.navigate(['login']);
            return false;
        }
    }
}