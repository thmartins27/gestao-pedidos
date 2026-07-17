import { Component, OnInit, signal } from '@angular/core';
import { PedidoResumo } from '../../models/pedido.model';
import { StatusPedido } from '../../models/status-pedido.model';
import { PedidoService } from '../../core/services/pedido.service';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { StatusChip } from '../../shared/components/status-chip/status-chip';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { EstadoVazio } from '../../shared/components/estado-vazio/estado-vazio';
import { EstadoErro } from '../../shared/components/estado-erro/estado-erro';
import { EstadoCarregando } from '../../shared/components/estado-carregando/estado-carregando';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe, CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.html',
  styleUrl: './pedidos.scss',
  imports: [
    MatTableModule,
    MatSelectModule,
    MatPaginatorModule,
    FormsModule,
    MatFormFieldModule,
    StatusChip,
    EstadoVazio,
    EstadoErro,
    EstadoCarregando,
    MatIconModule,
    MatButtonModule,
    DatePipe,
    CurrencyPipe,
  ],
})
export class Pedidos implements OnInit {
  colunas = ['numero', 'cliente', 'data', 'status', 'valorTotal', 'acoes'];

  pedidos = signal<PedidoResumo[]>([]);
  carregando = signal(false);
  erro = signal(false);

  filtroStatus = signal<StatusPedido | null>(null);
  pagina = signal(1);
  pageSize = signal(10);
  totalItens = signal(0);

  statusOpcoes: StatusPedido[] = ['Pendente', 'Pago', 'Cancelado'];

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.carregarPedidos();
  }

  carregarPedidos(): void {
    this.carregando.set(true);
    this.erro.set(false);

    this.pedidoService
      .listar(this.pagina(), this.pageSize(), this.filtroStatus() ?? undefined)
      .subscribe({
        next: (resultado) => {
          this.pedidos.set(resultado.items);
          this.totalItens.set(resultado.totalItems);
          this.carregando.set(false);
        },
        error: () => {
          this.erro.set(true);
          this.carregando.set(false);
        },
      });
  }

  onMudarFiltro(status: StatusPedido | null): void {
    this.filtroStatus.set(status);
    this.pagina.set(1);
    this.carregarPedidos();
  }

  onMudarPagina(evento: PageEvent): void {
    this.pagina.set(evento.pageIndex + 1);
    this.pageSize.set(evento.pageIndex);
    this.carregarPedidos();
  }
}
