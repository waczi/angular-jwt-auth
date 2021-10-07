import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from './components/base.component';
import { AuthStorage } from './services/auth/auth.storage';
import { BackendApi } from './services/http/backend.api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent extends BaseComponent {
  title = 'jwt-auth';

  constructor(
    router : Router,
    private backendApi : BackendApi,
    private authStorage: AuthStorage) {
    super(router)
   }

  public async sigonOut() {
    await this.backendApi.signOut()
    this.authStorage.removeCurrentTokens()
    this.router.navigate(['login'])
  }

  public isUserLoggedIn(): boolean {
    var accessToken = this.authStorage.getAccessToken()

    if(accessToken){
      return true
    }
    else{
      return false
    }
  }
}
