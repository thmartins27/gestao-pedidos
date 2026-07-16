using GestaoPedidos.Api.Exceptions;

namespace GestaoPedidos.Api.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int EstoqueAtual { get; private set; }

    private Produto() { }

    public Produto(string nome, decimal preco, int estoqueAtual)
    {
        Nome = nome;
        Preco = preco;

        if (estoqueAtual < 0)
            throw new ArgumentException("O estoque não pode ser negativo.", nameof(estoqueAtual));
        EstoqueAtual = estoqueAtual;
    }

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A Quantidade deve ser maior que zero.", nameof(quantidade));
        if (quantidade > EstoqueAtual)
            throw new EstoqueInsuficienteException(Nome, EstoqueAtual, quantidade);

        EstoqueAtual -= quantidade;
    }
}