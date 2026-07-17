import { HttpInterceptorFn } from '@angular/common/http';
import { tap } from 'rxjs';

export const loggingInterceptor: HttpInterceptorFn = (req, next) => {
  const started = Date.now();
  console.log(`-> ${req.method} ${req.url}`);

  return next(req).pipe(
    tap({
      next: () => {
        console.log(`← ${req.method} ${req.url} (${Date.now() - started}ms)`);
      },
      error: (err) => {
        console.log(`✗ ${req.method} ${req.url} (${Date.now() - started}ms)`, err);
      },
    }),
  );
};
