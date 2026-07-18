import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { DashboardResumo } from '../../models/dashboard.model';

const API_URL = `${environment.apiUrl}/dashboard`;

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private http: HttpClient) {}

  obterResumo(): Observable<DashboardResumo> {
    return this.http.get<DashboardResumo>(API_URL);
  }
}
