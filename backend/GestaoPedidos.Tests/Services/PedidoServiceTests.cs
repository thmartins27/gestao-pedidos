using GestaoPedidos.Api.DTOs.Pedidos;
using GestaoPedidos.Api.Exceptions;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Models.Enums;
using GestaoPedidos.Api.Repositories;
using GestaoPedidos.Api.Services;
using Moq;

public class PedidoServiceTests
{
    private readonly Mock<IClienteRepository> _clienteRepo = new();
    private readonly Mock<IProdutoRepository> _produtoRepo = new();
    private readonly Mock<IPedidoRepository> _pedidoRepo = new();
    private readonly IPedidoService _service;


    public PedidoServiceTests()
    {
        _service = new PedidoService(_clienteRepo.Object, _produtoRepo.Object, _pedidoRepo.Object);
    }


    private static Cliente ClienteFake(int id = 1)
    {
        var cliente = new Cliente("Cliente Teste", "cliente@teste.com") { Id = id };
        return cliente;
    }

    private static Produto ProdutoFake(int id, int estoque, decimal preco = 10m)
    {
        var produto = new Produto("Produto teste", preco, estoque) { Id = id };
        return produto;
    }

    [Fact]
    public async Task CriarAsync_ComEstoqueSuficiente_DeveBaixarEstoqueEPersistir()
    {
        var cliente = ClienteFake(1);
        var produto = ProdutoFake(id: 10, estoque: 5, preco: 20m);

        _clienteRepo.Setup(r => r.ObterPorIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        _produtoRepo.Setup(r => r.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Produto> { produto });

        _pedidoRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken _) =>
            {
                var p = new Pedido(cliente.Id);
                typeof(Pedido)
                    .GetProperty(
                        nameof(Pedido.Cliente))!.SetValue(p, cliente);
                p.AdicionarItem(new ItemPedido(produto, p.Id, 3));
                return p;
            });

        var dto = new CreatePedidoDto
        {
            ClienteId = 1,
            Itens = [new CreatePedidoItemDto { ProdutoId = 10, Quantidade = 3 }]
        };

        var resultado = await _service.CriarAsync(dto, TestContext.Current.CancellationToken);

        Assert.Equal(2, produto.EstoqueAtual); // verifica se o estoque teve baixa
        Assert.Equal("Pendente", resultado.Status); // pedido deve sempre iniciar como pendente

        _pedidoRepo.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepo.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_ComEstoqueInsuficiente_DeveLancarExcecaoENaoPersistir()
    {
        var cliente = ClienteFake(1);
        var produto = ProdutoFake(id: 10, estoque: 5);

        _clienteRepo.Setup(r => r.ObterPorIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);
        _produtoRepo.Setup(r => r.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Produto> { produto });


        var dto = new CreatePedidoDto
        {
            ClienteId = 1,
            Itens = [new CreatePedidoItemDto {
                ProdutoId = 10, Quantidade = 6 // estoque com 1 a mais
            }]
        };

        await Assert.ThrowsAsync<EstoqueInsuficienteException>(() => _service.CriarAsync(dto, TestContext.Current.CancellationToken));

        _pedidoRepo.Verify(r => r.SalvarAlteracoesAsync(
            It.IsAny<CancellationToken>()
        ), Times.Never);
    }

    [Fact]
    public async Task CriarAsync_ComClienteInexistente_DeveLancarNotFound()
    {
        _clienteRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        var dto = new CreatePedidoDto
        {
            ClienteId = 999,
            Itens = [new CreatePedidoItemDto { ProdutoId = 10, Quantidade = 1 }]
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CriarAsync(dto, TestContext.Current.CancellationToken));

        // Nem chegou a buscar produtos nem a salvar
        _produtoRepo.Verify(r => r.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()), Times.Never);
        _pedidoRepo.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CriarAsync_ComProdutoInexistente_DeveLancarNotFound()
    {
        // cliente existe, mas a busca de produtos volta vazia (id não encontrado)
        _clienteRepo.Setup(r => r.ObterPorIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ClienteFake(1));
        _produtoRepo.Setup(r => r.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Produto>()); // nenhum produto encontrado

        var dto = new CreatePedidoDto
        {
            ClienteId = 1,
            Itens = [new CreatePedidoItemDto { ProdutoId = 404, Quantidade = 1 }]
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CriarAsync(dto, TestContext.Current.CancellationToken));
        _pedidoRepo.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }


    [Fact]
    public async Task AtualizarStatusAsync_ComTransicaoInvalida_DeveLancarInvalidOperation()
    {
        // pedido já Cancelado; tentar ir para Pago é inválido pelo domínio
        var pedido = new Pedido(1);
        pedido.MudarStatus(StatusPedido.Cancelado); // Pendente -> Cancelado (válido)

        _pedidoRepo.Setup(r => r.ObterParaAtualizacaoAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        var dto = new UpdateStatusPedidoDto { NovoStatus = StatusPedido.Pago };

        await Assert.ThrowsAsync<TransicaoStatusInvalidaException>(
            () => _service.AtualizarStatusAsync(1, dto, TestContext.Current.CancellationToken));

        _pedidoRepo.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

}