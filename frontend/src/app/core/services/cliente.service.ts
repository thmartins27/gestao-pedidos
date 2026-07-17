import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Cliente, CreateCliente, UpdateCliente } from '../../models/cliente.model';
import { PagedResult } from '../../models/paged-result.model';

const API_URL = `${environment.apiUrl}/clientes`;

@Injectable({ providedIn: 'root' })
export class ClienteService {
  constructor(private http: HttpClient) {}

  listar(page = 1, pageSize = 10): Observable<PagedResult<Cliente>> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<PagedResult<Cliente>>(API_URL, { params });
  }

  buscarPorId(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${API_URL}/${id}`);
  }

  criar(dto: CreateCliente): Observable<Cliente> {
    return this.http.post<Cliente>(API_URL, dto);
  }

  atualizar(id: number, dto: UpdateCliente): Observable<Cliente> {
    return this.http.put<Cliente>(`${API_URL}/${id}`, dto);
  }

  remover(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/${id}`);
  }
}
