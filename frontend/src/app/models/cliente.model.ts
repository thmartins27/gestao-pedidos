export interface Cliente {
  id: number;
  nome: string;
  email: string;
}

export interface CreateCliente {
  nome: string;
  email: string;
}

export type UpdateCliente = CreateCliente;
