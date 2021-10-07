import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthStorage } from './auth.storage';

@Injectable({
    providedIn: 'root'
})
export class AuthNotRequiredGuard implements CanActivate {

    constructor(
        private authStorage: AuthStorage,
        private router: Router) { }

    async canActivate(): Promise<boolean> {
        const token = this.authStorage.getAccessToken();
        if (token) {
            this.router.navigate(['customer-list']);
            return false;
        }
        else {
            return true;
        }
    }
}