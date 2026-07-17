export interface Produto {
  id: number;
  nome: string;
  preco: string;
  estoqueAtual: number;
}

export interface CreateProduto {
  nome: string;
  preco: string;
  estoqueAtual: number;
}

export interface UpdateProduto {
  nome: string;
  preco: number;
}
