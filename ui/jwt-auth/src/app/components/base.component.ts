import { Router } from '@angular/router';
import { RefreshTokenExpiredError } from 'src/app/services/auth/auth.token-expired.error';

export class BaseComponent {

  constructor(protected router: Router) { }

  protected handleHttpError(error: any) {

    if (error instanceof RefreshTokenExpiredError) {
      this.router.navigate(['login']);
    }
    else {
      alert("Unexpected error.");
    }
  }
}
