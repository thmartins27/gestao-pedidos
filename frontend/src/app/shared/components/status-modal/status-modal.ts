import { Component, inject, signal } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { StatusPedido } from '../../../models/status-pedido.model';
import { PedidoService } from '../../../core/services/pedido.service';
import { StatusChip } from '../status-chip/status-chip';

export interface StatusModalData {
  id: number;
  nomeCliente: string;
  status: StatusPedido;
}

const TRANSICOES: Record<StatusPedido, StatusPedido[]> = {
  Pendente: ['Pago', 'Cancelado'],
  Pago: ['Cancelado'],
  Cancelado: [],
};

@Component({
  selector: 'app-status-modal',
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    StatusChip,
  ],
  templateUrl: './status-modal.html',
  styleUrl: './status-modal.scss',
})
export class StatusModal {
  readonly data = inject<StatusModalData>(MAT_DIALOG_DATA);

  private dialogRef = inject(MatDialogRef<StatusModal>);
  private pedidoService = inject(PedidoService);

  opcoes = TRANSICOES[this.data.status];

  selecionado = signal<StatusPedido | null>(null);
  carregando = signal(false);
  erro = signal<string | null>(null);

  fechar(): void {
    this.dialogRef.close();
  }

  confirmar(): void {
    const novoStatus = this.selecionado();
    if (!novoStatus || this.carregando()) {
      return;
    }

    this.carregando.set(true);
    this.erro.set(null);

    this.pedidoService.atualizarStatus(this.data.id, { novoStatus }).subscribe({
      next: (pedido) => this.dialogRef.close(pedido),
      error: (err: Error) => {
        this.erro.set(err.message);
        this.carregando.set(false);
      },
    });
  }
}
