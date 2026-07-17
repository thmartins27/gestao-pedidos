import { ApplicationConfig, LOCALE_ID, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { MatPaginatorIntl } from '@angular/material/paginator';

import { routes } from './app.routes';
import { loggingInterceptor } from './core/interceptors/logging.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { paginatorIntlPtBr } from './core/paginator-intl';

registerLocaleData(localePt);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([loggingInterceptor, errorInterceptor])),
    provideAnimationsAsync(),
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    { provide: MatPaginatorIntl, useFactory: paginatorIntlPtBr },
  ],
};
