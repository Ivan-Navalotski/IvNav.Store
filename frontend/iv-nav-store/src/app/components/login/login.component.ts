import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-login',
  template: '<button (click)="login()">Login</button>',
})
export class LoginComponent {
  constructor(private oauthService: OAuthService) { }

  login(): void {
    console.log("login")
    //this.oauthService.tryLogin();
    this.oauthService.initCodeFlow();
  }
}
