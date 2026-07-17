import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { StatusPedido } from '../../models/status-pedido.model';
import { Observable } from 'rxjs';
import { PagedResult } from '../../models/paged-result.model';
import { CreatePedido, Pedido, PedidoResumo, UpdateStatusPedido } from '../../models/pedido.model';

const API_URL = `${environment.apiUrl}/pedidos`;

@Injectable({ providedIn: 'root' })
export class PedidoService {
  constructor(private readonly http: HttpClient) {}

  listar(
    page: number = 1,
    pageSize: number = 10,
    status?: StatusPedido,
  ): Observable<PagedResult<PedidoResumo>> {
    let params = new HttpParams().set('page', page).set('pageSize', pageSize);

    if (status) {
      params = params.set('status', status);
    }
    return this.http.get<PagedResult<PedidoResumo>>(API_URL, { params });
  }

  buscarPorId(id: number): Observable<Pedido> {
    return this.http.get<Pedido>(`${API_URL}/${id}`);
  }

  criar(dto: CreatePedido): Observable<Pedido> {
    return this.http.post<Pedido>(API_URL, dto);
  }

  atualizarStatus(id: number, dto: UpdateStatusPedido): Observable<Pedido> {
    return this.http.patch<Pedido>(`${API_URL}/${id}`, dto);
  }
}
