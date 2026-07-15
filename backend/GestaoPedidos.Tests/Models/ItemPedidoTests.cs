using GestaoPedidos.Api.Models;

namespace GestaoPedidos.Tests.Models;

public class ItemPedidoTests
{
    private static Produto CriarProduto() => new("Fone de ouvido", 99.90m, 10);

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void DefinirQuantidade_ComValorInvalido_LancaArgumentException(int quantidade)
    {
        var item = new ItemPedido(CriarProduto(), pedidoId: 1, quantidade: 1);
        Assert.Throws<ArgumentException>(() => item.DefinirQuantidade(quantidade));
    }

    [Fact]
    public void DefinirQuantidade_ComValorValido_AtualizaQuantidade()
    {
        var item = new ItemPedido(CriarProduto(), pedidoId: 1, quantidade: 1);
        item.DefinirQuantidade(4);
        Assert.Equal(4, item.Quantidade);
    }

    [Fact]
    public void AdicionarQuantidade_ComValorValido_SomaAQuantidadeExistente()
    {
        var item = new ItemPedido(CriarProduto(), pedidoId: 1, quantidade: 3);
        item.AdicionarQuantidade(2);
        Assert.Equal(5, item.Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AdicionarQuantidade_ComValorInvalido_LancaArgumentException(int quantidade)
    {
        var item = new ItemPedido(CriarProduto(), pedidoId: 1, quantidade: 3);
        Assert.Throws<ArgumentException>(() => item.AdicionarQuantidade(quantidade));
    }

    [Fact]
    public void CalcularSubtotal_MultiplicaQuantidadePeloPrecoUnitario()
    {
        var item = new ItemPedido(CriarProduto(), pedidoId: 1, quantidade: 3);
        Assert.Equal(299.70m, item.CalcularSubtotal());
    }
}