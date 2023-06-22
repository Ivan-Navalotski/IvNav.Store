import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { filter, map } from 'rxjs/operators';
import { authCodeFlowConfig } from './../auth.config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
})
export class AppComponent {
  title = 'Quickstart Demo';

  constructor(private oauthService: OAuthService) {
    this.oauthService.configure(authCodeFlowConfig);

    // Automatically load user profile
    this.oauthService.events
      .pipe(filter((e) => e.type === 'token_received'))
      .subscribe((_) => {
        console.debug('state', this.oauthService.state);
        this.oauthService.loadUserProfile();

        const scopes = this.oauthService.getGrantedScopes();
        console.debug('scopes', scopes);
      });

    this.oauthService.loadDiscoveryDocument().then((_) => {
      // if (useHash) {
      //   this.router.navigate(['/']);
      // }

      console.log('hasValidAccessToken : ', this.oauthService.hasValidAccessToken());
      console.log('hasValidIdToken : ', this.oauthService.hasValidIdToken());
      console.log('getAccessTokenExpiration : ', this.oauthService.getAccessTokenExpiration());
      console.log('getAccessToken : ', this.oauthService.getAccessToken());
      console.log('getIdToken : ', this.oauthService.getIdToken());
    });

    this.oauthService.initImplicitFlow();

    // Optional
    this.oauthService.setupAutomaticSilentRefresh();

    this.oauthService.tryLogin();

    // Automatically load user profile
    this.oauthService.events
      .pipe(filter((e) => e.type === 'token_received'))
      .subscribe((_) => this.oauthService.loadUserProfile());
  }

  get userName(): string {
    const claims = this.oauthService.getIdentityClaims();
    if (!claims) return "";
    return claims['given_name'];
  }

  get idToken(): string {
    return this.oauthService.getIdToken();
  }

  get accessToken(): string {
    return this.oauthService.getAccessToken();
  }

  refresh() {
    this.oauthService.refreshToken();
  }
}
