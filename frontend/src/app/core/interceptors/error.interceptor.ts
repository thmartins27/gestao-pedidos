import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export interface ApiErrorBody {
  status: number;
  title: string;
  detail: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      const body = err.error as ApiErrorBody | undefined;
      const mensagem = body?.detail ?? 'Ococrreu um erro inesperado. Tente novamente.';

      console.error(`Erro na requisição ${req.method} ${req.url}: `, mensagem);

      return throwError(() => new Error(mensagem));
    }),
  );
};
