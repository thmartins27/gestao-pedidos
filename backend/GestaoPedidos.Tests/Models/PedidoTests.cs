using GestaoPedidos.Api.Exceptions;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Tests.Models;

public class PedidoTests
{
    private static Produto CriarProduto(
        string nome = "Teclado", decimal preco = 150m, int estoque = 100, int id = 1)
        => new(nome, preco, estoque) { Id = id };

    private static ItemPedido CriarItem(Produto produto, int quantidade)
        => new(produto, pedidoId: 1, quantidade: quantidade);

    // ---------- AdicionarItem ----------

    [Fact]
    public void AdicionarItem_ProdutoNovo_AdicionaNaColecao()
    {
        var pedido = new Pedido(clienteId: 1);
        var produto = CriarProduto();

        pedido.AdicionarItem(CriarItem(produto, 2));

        Assert.Single(pedido.Itens);
    }

    [Fact]
    public void AdicionarItem_ProdutoDuplicado_ConsolidaQuantidadeEmUmUnicoItem()
    {
        var pedido = new Pedido(clienteId: 1);
        var produto = CriarProduto();

        pedido.AdicionarItem(CriarItem(produto, 2));
        pedido.AdicionarItem(CriarItem(produto, 3));

        Assert.Single(pedido.Itens);
        Assert.Equal(5, pedido.Itens.First().Quantidade);
    }

    [Fact]
    public void AdicionarItem_RecalculaValorTotalAutomaticamente()
    {
        var pedido = new Pedido(clienteId: 1);
        var teclado = CriarProduto(nome: "Teclado", preco: 150m, id: 1);
        var mouse = CriarProduto(nome: "Mouse", preco: 80m, id: 2);

        pedido.AdicionarItem(CriarItem(teclado, 2));
        pedido.AdicionarItem(CriarItem(mouse, 1));

        Assert.Equal(380m, pedido.ValorTotal);
    }

    // ---------- MudarStatus: transições válidas ----------

    [Theory]
    [InlineData(StatusPedido.Pago)]
    [InlineData(StatusPedido.Cancelado)]
    public void MudarStatus_DePendente_PermitePagoOuCancelado(StatusPedido destino)
    {
        var pedido = new Pedido(clienteId: 1);

        pedido.MudarStatus(destino);

        Assert.Equal(destino, pedido.Status);
    }

    [Fact]
    public void MudarStatus_DePagoParaCancelado_Permitido()
    {
        var pedido = new Pedido(clienteId: 1);
        pedido.MudarStatus(StatusPedido.Pago);

        pedido.MudarStatus(StatusPedido.Cancelado);

        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
    }

    // ---------- MudarStatus: transições inválidas ----------

    [Fact]
    public void MudarStatus_DePagoParaPendente_LancaInvalidOperationException()
    {
        var pedido = new Pedido(clienteId: 1);
        pedido.MudarStatus(StatusPedido.Pago);

        Assert.Throws<TransicaoStatusInvalidaException>(
            () => pedido.MudarStatus(StatusPedido.Pendente));
    }

    [Theory]
    [InlineData(StatusPedido.Pendente)]
    [InlineData(StatusPedido.Pago)]
    public void MudarStatus_DeCancelado_NaoPermiteNenhumaTransicao(StatusPedido destino)
    {
        var pedido = new Pedido(clienteId: 1);
        pedido.MudarStatus(StatusPedido.Cancelado);

        Assert.Throws<TransicaoStatusInvalidaException>(
            () => pedido.MudarStatus(destino));
    }

    [Fact]
    public void MudarStatus_ParaOMesmoStatus_LancaInvalidOperationException()
    {
        var pedido = new Pedido(clienteId: 1);

        Assert.Throws<TransicaoStatusInvalidaException>(
            () => pedido.MudarStatus(StatusPedido.Pendente));
    }
}