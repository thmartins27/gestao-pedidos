import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { forkJoin } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Cliente } from '../../models/cliente.model';
import { Produto } from '../../models/produto.model';
import { ClienteService } from '../../core/services/cliente.service';
import { ProdutoService } from '../../core/services/produto.service';
import { PedidoService } from '../../core/services/pedido.service';
import { EstadoCarregando } from '../../shared/components/estado-carregando/estado-carregando';
import { EstadoErro } from '../../shared/components/estado-erro/estado-erro';

interface ItemNovoPedido {
  produto: Produto;
  quantidade: number;
}

@Component({
  selector: 'app-novo-pedido',
  imports: [
    FormsModule,
    CurrencyPipe,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    EstadoCarregando,
    EstadoErro,
  ],
  templateUrl: './novo-pedido.html',
  styleUrl: './novo-pedido.scss',
})
export class NovoPedido implements OnInit {
  colunas = ['produto', 'quantidade', 'precoUnitario', 'subtotal', 'remover'];

  clientes = signal<Cliente[]>([]);
  produtos = signal<Produto[]>([]);
  carregando = signal(false);
  erro = signal(false);

  clienteId = signal<number | null>(null);
  produtoSelecionado = signal<Produto | null>(null);
  quantidade = signal(1);
  itens = signal<ItemNovoPedido[]>([]);
  salvando = signal(false);

  total = computed(() =>
    this.itens().reduce((soma, item) => soma + item.produto.preco * item.quantidade, 0),
  );

  private clienteService = inject(ClienteService);
  private produtoService = inject(ProdutoService);
  private pedidoService = inject(PedidoService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados(): void {
    this.carregando.set(true);
    this.erro.set(false);

    forkJoin({
      clientes: this.clienteService.listar(1, 100),
      produtos: this.produtoService.listar(1, 100),
    }).subscribe({
      next: ({ clientes, produtos }) => {
        this.clientes.set(clientes.items);
        this.produtos.set(produtos.items);
        this.carregando.set(false);
      },
      error: () => {
        this.erro.set(true);
        this.carregando.set(false);
      },
    });
  }

  adicionarItem(): void {
    const produto = this.produtoSelecionado();
    const quantidade = this.quantidade();
    if (!produto || quantidade < 1) {
      return;
    }

    const itens = [...this.itens()];
    const existente = itens.find((item) => item.produto.id === produto.id);
    if (existente) {
      existente.quantidade += quantidade;
    } else {
      itens.push({ produto, quantidade });
    }

    this.itens.set(itens);
    this.produtoSelecionado.set(null);
    this.quantidade.set(1);
  }

  removerItem(item: ItemNovoPedido): void {
    this.itens.set(this.itens().filter((i) => i !== item));
  }

  criarPedido(): void {
    const clienteId = this.clienteId();
    if (!clienteId || this.itens().length === 0 || this.salvando()) {
      return;
    }

    this.salvando.set(true);

    const dto = {
      clienteId,
      itens: this.itens().map((item) => ({
        produtoId: item.produto.id,
        quantidade: item.quantidade,
      })),
    };

    this.pedidoService.criar(dto).subscribe({
      next: () => {
        this.snackBar.open('Pedido criado com sucesso.', undefined, { duration: 4000 });
        this.router.navigate(['/pedidos']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message, 'Fechar', { duration: 6000 });
        this.salvando.set(false);
      },
    });
  }
}
