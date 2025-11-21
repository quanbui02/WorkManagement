import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'; // add this
import { providePrimeNG } from 'primeng/config'; // add this
import Aura from '@primeuix/themes/aura'; // add this
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideOAuthClient,OAuthService  } from 'angular-oauth2-oidc';
import { authInterceptor } from './core/interceptors/authorization-intercepter';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
       withInterceptors([authInterceptor])
    ),
    provideOAuthClient(),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura,
          options: {
            darkModeSelector: false || 'none', //'.my-app-dark', sau này tuỳ chỉnh darkmode ở đây ...
            cssLayer: {
            name: 'primeng',
            order: 'app-styles, primeng, another-css-library'
            }
          }
        },
    }),
  ]
};

