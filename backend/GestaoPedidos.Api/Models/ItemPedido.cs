namespace GestaoPedidos.Api.Models;

public class ItemPedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    public Produto Produto { get; private set; } = null!;

    private ItemPedido() { }

    public ItemPedido(Produto produto, int pedidoId, int quantidade)
    {
        PedidoId = pedidoId;
        Produto = produto;
        PrecoUnitario = produto.Preco;
        ProdutoId = produto.Id;
        DefinirQuantidade(quantidade);
    }

    public void DefinirQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantidade));

        Quantidade = quantidade;
    }

    public void AdicionarQuantidade(int quantidadeAdicional)
    {
        if (quantidadeAdicional <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantidadeAdicional));

        Quantidade += quantidadeAdicional;
    }

    public decimal CalcularSubtotal() => Quantidade * PrecoUnitario;
}