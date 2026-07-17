export interface Produto {
  id: number;
  nome: string;
  preco: number;
  estoqueAtual: number;
}

export interface CreateProduto {
  nome: string;
  preco: number;
  estoqueAtual: number;
}

export interface UpdateProduto {
  nome: string;
  preco: number;
}
