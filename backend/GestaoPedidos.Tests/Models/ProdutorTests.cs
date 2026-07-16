using GestaoPedidos.Api.Exceptions;
using GestaoPedidos.Api.Models;

namespace GestaoPedidos.Tests.Models;

public class ProdutoTests
{
    [Fact]
    public void EstoqueAtual_ValorNegativo_GeraErro()
    {
        Assert.Throws<ArgumentException>(() => new Produto("Caneta Bic", 2.20m, -1));
    }

    [Fact]
    public void EstoqueAtual_ValorZero_NaoGeraErro()
    {
        var produto = new Produto("Caneta Bic", 2.20m, 0);
        Assert.Equal(0, produto.EstoqueAtual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void BaixarEstoque_ComQuantidadeZeroOuNegativa_LancaArgumentException(int quantidade)
    {
        var produto = new Produto("Caneta Bic", 2.20m, 20);
        Assert.Throws<ArgumentException>(() => produto.BaixarEstoque(quantidade));
    }

    [Fact]
    public void BaixarEstoque_ComQuantidadeMaiorQueEstoque_LancaEstoqueInsuficienteException()
    {
        var produto = new Produto("Caneta Bic", 2.20m, 20);
        Assert.Throws<EstoqueInsuficienteException>(() => produto.BaixarEstoque(200));
    }
}