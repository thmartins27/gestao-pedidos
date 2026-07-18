import { StatusPedido } from './status-pedido.model';

export interface PedidosPorStatus {
  status: StatusPedido;
  quantidade: number;
}

export interface DashboardResumo {
  totalPedidos: number;
  valorTotalPedidos: number;
  pedidosPorStatus: PedidosPorStatus[];
}
