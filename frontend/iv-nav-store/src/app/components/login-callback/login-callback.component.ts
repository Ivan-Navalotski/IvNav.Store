import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-callback',
  template: '<div>Logging in...</div>',
})
export class LoginCallbackComponent implements OnInit {
  constructor(private oauthService: OAuthService) { }

  ngOnInit(): void {
    this.oauthService.loadDiscoveryDocumentAndTryLogin();

    console.log('hasValidAccessToken : ', this.oauthService.hasValidAccessToken());
    console.log('hasValidIdToken : ', this.oauthService.hasValidIdToken());
    console.log('getAccessTokenExpiration : ', this.oauthService.getAccessTokenExpiration());
    console.log('getAccessToken : ', this.oauthService.getAccessToken());
    console.log('getIdToken : ', this.oauthService.getIdToken());
  }
}
