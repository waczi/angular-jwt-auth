import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ErrorStatus } from 'src/app/dto/backend-error.response.body';
import { SignInRequestBody } from 'src/app/dto/sign-in.request.body';
import { BackendApi } from 'src/app/services/http/backend.api';
import { RequestStatus } from "src/app/dto/request-status";
import { AuthStorage } from 'src/app/services/auth/auth.storage';
import { Router } from '@angular/router';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent extends BaseComponent implements OnInit {

  public signInForm!: FormGroup;
  public submitted = false;
  public apiError! : string;

  constructor(
    private backendApi: BackendApi,
    private authStorage: AuthStorage,
    router : Router,
    formBuilder: FormBuilder) {
    super(router);
    this.signInForm = this.getSignInForm(formBuilder)
    this.apiError = ""
  }

  ngOnInit(): void {
  }

  public async submitSignInForm() {
    this.submitted = true
    this.apiError = ""
    if (!this.signInForm.valid) {
      return;
    }
    else {
      try {
        const body = new SignInRequestBody()
        body.email = this.email?.value
        body.password = this.password?.value
        const apiResponse = await this.backendApi.signIn(body)
        
        if (apiResponse.requestStatus === RequestStatus.Success) {
          this.authStorage.setNewTokens(apiResponse.body.token, apiResponse.body.refreshToken)
          this.router.navigate(["customer-list"])
        }
        else {
          switch (apiResponse.body.status) {
            case ErrorStatus.InvalidUserCredentials:
              this.apiError = "Invalid credentials."
              break;
            case ErrorStatus.UserNotFound:
              this.apiError = "Invalid credentials."
              break;
            default:
              this.apiError = "Invalid credentials."
              break;
          }
        }
      }
      catch (error) {
        this.handleHttpError(error)
      }
    }
  }

  //Validation stuff
  get email() {
    return this.signInForm.get('email')
  }

  get password() {
    return this.signInForm.get('password')
  }

  //Form factory method
  private getSignInForm(formBuilder: FormBuilder): FormGroup {
    return formBuilder.group({
      password: ['', [
        Validators.required
      ]],
      email: ['', [
        Validators.required
      ]]
    });
  }
}


