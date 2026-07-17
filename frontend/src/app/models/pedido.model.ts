import { StatusPedido } from './status-pedido.model';

export interface ItemPedido {
  produtoId: number;
  nomeProduto: string;
  quantidade: number;
  precoUnitario: number;
  subtotal: number;
}

export interface PedidoResumo {
  id: number;
  clienteId: number;
  nomeCliente: string;
  dataPedido: string;
  status: StatusPedido;
  valorTotal: number;
}

export interface Pedido extends PedidoResumo {
  itens: ItemPedido[];
}

export interface CreatePedidoItem {
  produtoId: number;
  quantidade: number;
}

export interface CreatePedido {
  clienteId: number;
  itens: CreatePedidoItem[];
}

export interface UpdateStatusPedido {
  novoStatus: StatusPedido;
}
