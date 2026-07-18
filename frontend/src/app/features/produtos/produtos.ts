import { Component, OnInit, signal } from '@angular/core';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { CurrencyPipe } from '@angular/common';
import { Produto } from '../../models/produto.model';
import { ProdutoService } from '../../core/services/produto.service';
import { EstadoVazio } from '../../shared/components/estado-vazio/estado-vazio';
import { EstadoErro } from '../../shared/components/estado-erro/estado-erro';
import { EstadoCarregando } from '../../shared/components/estado-carregando/estado-carregando';

@Component({
  selector: 'app-produtos',
  templateUrl: './produtos.html',
  styleUrl: './produtos.scss',
  imports: [
    MatTableModule,
    MatPaginatorModule,
    CurrencyPipe,
    EstadoVazio,
    EstadoErro,
    EstadoCarregando,
  ],
})
export class Produtos implements OnInit {
  colunas = ['nome', 'preco', 'estoqueAtual'];

  produtos = signal<Produto[]>([]);
  carregando = signal(false);
  erro = signal(false);

  pagina = signal(1);
  pageSize = signal(10);
  totalItens = signal(0);

  constructor(private produtoService: ProdutoService) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.carregando.set(true);
    this.erro.set(false);

    this.produtoService.listar(this.pagina(), this.pageSize()).subscribe({
      next: (resultado) => {
        this.produtos.set(resultado.items);
        this.totalItens.set(resultado.totalItems);
        this.carregando.set(false);
      },
      error: () => {
        this.erro.set(true);
        this.carregando.set(false);
      },
    });
  }

  onMudarPagina(evento: PageEvent): void {
    this.pagina.set(evento.pageIndex + 1);
    this.pageSize.set(evento.pageSize);
    this.carregarProdutos();
  }
}
