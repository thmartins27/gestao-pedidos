import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Produto, CreateProduto, UpdateProduto } from '../../models/produto.model';
import { PagedResult } from '../../models/paged-result.model';

const API_URL = `${environment.apiUrl}/produtos`;

@Injectable({ providedIn: 'root' })
export class ProdutoService {
  constructor(private http: HttpClient) {}

  listar(page = 1, pageSize = 10): Observable<PagedResult<Produto>> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<PagedResult<Produto>>(API_URL, { params });
  }

  buscarPorId(id: number): Observable<Produto> {
    return this.http.get<Produto>(`${API_URL}/${id}`);
  }

  criar(dto: CreateProduto): Observable<Produto> {
    return this.http.post<Produto>(API_URL, dto);
  }

  atualizar(id: number, dto: UpdateProduto): Observable<Produto> {
    return this.http.put<Produto>(`${API_URL}/${id}`, dto);
  }

  remover(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/${id}`);
  }
}
