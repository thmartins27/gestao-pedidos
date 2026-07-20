import { provideHttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { PedidoService } from './pedido.service';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { PagedResult } from '../../models/paged-result.model';
import { CreatePedido, PedidoResumo } from '../../models/pedido.model';

describe('PedidoService', () => {
  let service: PedidoService;
  let httpMock: HttpTestingController;

  const API_URL = `${environment.apiUrl}/pedidos`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PedidoService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(PedidoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve buscar pedidos paginados', () => {
    const mockResponse: PagedResult<PedidoResumo> = {
      items: [
        {
          id: 1,
          clienteId: 1,
          nomeCliente: 'Maria Oliveira',
          dataPedido: '2026-07-15T00:00:00Z',
          status: 'Pago',
          valorTotal: 1050,
        },
      ],
      page: 1,
      pageSize: 10,
      totalItems: 1,
      totalPages: 1,
    };

    service.listar(1, 10).subscribe((resultado) => {
      expect(resultado.items.length).toBe(1);
      expect(resultado.items[0].nomeCliente).toBe('Maria Oliveira');
    });

    const req = httpMock.expectOne((r) => r.url === API_URL && r.params.get('page') === '1');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('deve filtrar pedidos por status', () => {
    service.listar(1, 10, 'Pendente').subscribe();

    const req = httpMock.expectOne(
      (r) => r.url === API_URL && r.params.get('status') == 'Pendente',
    );

    req.flush({ items: [], page: 1, pageSize: 10, totalItems: 0, totalPages: 0 });
  });

  it('deve criar um pedido', () => {
    const novoPedido: CreatePedido = { clienteId: 1, itens: [{ produtoId: 2, quantidade: 3 }] };

    const mockResposta = {
      id: 10,
      clienteId: 1,
      nomeCliente: 'Maria',
      dataPedido: '...',
      status: 'Pendente',
      valorTotal: 300,
      itens: [{ produtoId: 2, nomeProduto: 'X', quantidade: 3, precoUnitario: 100, subtotal: 300 }],
    };

    let chamou = false;
    service.criar(novoPedido).subscribe((r) => {
      chamou = true;
      expect(r.itens.length).toBe(1);
    });

    const req = httpMock.expectOne(API_URL);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(novoPedido);
    req.flush(mockResposta);
    expect(chamou).toBe(true);
  });
});
