import { Component, input } from '@angular/core';
import { StatusPedido } from '../../../models/status-pedido.model';

@Component({
  selector: 'app-status-chip',
  imports: [],
  templateUrl: './status-chip.html',
  styleUrl: './status-chip.scss',
})
export class StatusChip {
  status = input.required<StatusPedido>();
}
