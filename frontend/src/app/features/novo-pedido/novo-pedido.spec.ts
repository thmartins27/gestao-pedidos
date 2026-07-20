import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NovoPedido } from './novo-pedido';
import { of, throwError } from 'rxjs';
import { ClienteService } from '../../core/services/cliente.service';
import { ProdutoService } from '../../core/services/produto.service';
import { PedidoService } from '../../core/services/pedido.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Produto } from '../../models/produto.model';

describe('novo-pedido', () => {
  let component: NovoPedido;
  let fixure: ComponentFixture<NovoPedido>;

  const clienteService = {
    listar: vi.fn(),
  };

  const produtoService = {
    listar: vi.fn(),
  };

  const pedidoService = {
    criar: vi.fn(),
  };

  const snackBar = {
    open: vi.fn(),
  };

  const router = {
    navigate: vi.fn(),
  };

  beforeEach(async () => {
    vi.clearAllMocks();

    clienteService.listar.mockReturnValue(of({ items: [] }));

    produtoService.listar.mockReturnValue(of({ items: [] }));

    await TestBed.configureTestingModule({
      imports: [NovoPedido],
      providers: [
        { provide: ClienteService, useValue: clienteService },
        { provide: ProdutoService, useValue: produtoService },
        { provide: PedidoService, useValue: pedidoService },
        { provide: MatSnackBar, useValue: snackBar },
        { provide: Router, useValue: router },
      ],
    }).compileComponents();

    fixure = TestBed.createComponent(NovoPedido);
    component = fixure.componentInstance;
  });

  it('deve criar', () => {
    expect(component).toBeTruthy();
  });

  it('deve carregar clientes e produtos', () => {
    clienteService.listar.mockReturnValue(
      of({
        items: [
          {
            id: 1,
            nome: 'Thiago',
          },
        ],
      }),
    );

    produtoService.listar.mockReturnValue(
      of({
        items: [
          {
            id: 1,
            nome: 'Notebook',
            preco: 100,
            estoqueAtual: 5,
          },
        ],
      }),
    );

    fixure.detectChanges();
    expect(clienteService.listar).toHaveBeenCalled();
    expect(produtoService.listar).toHaveBeenCalled();

    expect(component.clientes()).toHaveLength(1);
    expect(component.produtos()).toHaveLength(1);
  });

  it('deve marcar erro quando o carregamento falhar', () => {
    clienteService.listar.mockReturnValue(throwError(() => new Error()));
    fixure.detectChanges();
    expect(component.erro()).toBe(true);
  });


  it("deve adicionar item", () => {
    const produto: Produto = {
      id: 1,
      estoqueAtual: 10,
      preco: 100,
      nome: 'Notebook'
    }

    component.form.controls.produtoSelecionado.setValue(produto);
    component.form.controls.quantidade.setValue(2);

    component.adicionarItem()

    expect(component.itens()).toHaveLength(1);
    expect(component.total()).toBe(200);
  })



  it('deve criar um pedido', () => {
    pedidoService.criar.mockReturnValue(of({}));
    component.form.controls.clienteId.setValue(1);
    component.itens.set([{
      produto: {
        id: 5,
        nome: 'Mouse',
        preco: 100,
        estoqueAtual: 3
      },
      quantidade: 2
    }]);

    component.criarPedido();

    expect(pedidoService.criar).toHaveBeenCalled();
    expect(router.navigate)
      .toHaveBeenCalledWith(['/pedidos'])
  })

});
