import { AuthConfig } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
  issuer: 'https://localhost:7188',
  redirectUri: window.location.origin + '',
  clientId: 'PortalClient',
  responseType: 'code',
  scope: 'WebApi',
  showDebugInformation: true,
  timeoutFactor: 0.01,
};
