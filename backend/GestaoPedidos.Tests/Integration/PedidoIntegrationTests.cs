using System.Net;
using System.Net.Http.Json;
using GestaoPedidos.Api.DTOs.Clientes;
using GestaoPedidos.Api.DTOs.Pedidos;
using GestaoPedidos.Api.DTOs.Produtos;
using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Tests.Integration;

[Collection("Integration")]
public class PedidoIntegrationTests
{
    private readonly HttpClient _client;

    public PedidoIntegrationTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<int> CriarClienteAsync(string email)
    {
        var dto = new CreateClienteDto { Nome = "Cliente Integração", Email = email };
        var resp = await _client.PostAsJsonAsync("/api/clientes", dto, TestContext.Current.CancellationToken);
        resp.EnsureSuccessStatusCode();
        var criado = await resp.Content.ReadFromJsonAsync<ClienteDto>(TestContext.Current.CancellationToken);
        return criado!.Id;
    }

    private async Task<int> CriarProdutoAsync(int estoque, decimal preco = 10m)
    {
        var dto = new CreateProdutoDto { Nome = "Produto Integração", Preco = preco, EstoqueAtual = estoque };
        var resp = await _client.PostAsJsonAsync("/api/produtos", dto, TestContext.Current.CancellationToken);
        resp.EnsureSuccessStatusCode();
        var criado = await resp.Content.ReadFromJsonAsync<ProdutoDto>(TestContext.Current.CancellationToken);
        return criado!.Id;
    }

    [Fact]
    public async Task CriarPedido_ComEstoqueSuficiente_RetornaCreatedEBaixaEstoque()
    {
        var clienteId = await CriarClienteAsync("feliz@teste.com");
        var produtoId = await CriarProdutoAsync(estoque: 5, preco: 20m);

        var pedido = new CreatePedidoDto
        {
            ClienteId = clienteId,
            Itens = [new CreatePedidoItemDto { ProdutoId = produtoId, Quantidade = 3 }]
        };

        var response = await _client.PostAsJsonAsync("/api/pedidos", pedido, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var criado = await response.Content.ReadFromJsonAsync<PedidoDto>(TestContext.Current.CancellationToken);

        Assert.NotNull(criado);
        Assert.Equal(60m, criado!.ValorTotal);
        Assert.Equal("Pendente", criado!.Status);
        Assert.True(criado.Id > 0);

        // Verifica se o Estoque foi atualizado
        var produtoResp = await _client.GetAsync($"/api/produtos/{produtoId}", TestContext.Current.CancellationToken);
        var produto = await produtoResp.Content.ReadFromJsonAsync<ProdutoDto>(TestContext.Current.CancellationToken);

        Assert.Equal(2, produto!.EstoqueAtual);
    }

    [Fact]
    public async Task CriarPedido_ComEstoqueInsuficiente_RetornaBadRequest()
    {
        var clienteId = await CriarClienteAsync("insuficiente@teste.com");
        var produtoId = await CriarProdutoAsync(estoque: 5);

        var pedido = new CreatePedidoDto
        {
            ClienteId = clienteId,
            Itens = [new CreatePedidoItemDto { ProdutoId = produtoId, Quantidade = 6 }]
        };

        var response = await _client.PostAsJsonAsync("/api/pedidos", pedido, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var corpo = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Contains("Estoque insuficiente", corpo);
    }

    [Fact]
    public async Task AtualizarStatus_DePendenteParaPago_RetornaOk()
    {

        var clienteId = await CriarClienteAsync("status@teste.com");
        var produtoId = await CriarProdutoAsync(estoque: 5);

        var pedido = new CreatePedidoDto
        {
            ClienteId = clienteId,
            Itens = [new CreatePedidoItemDto { ProdutoId = produtoId, Quantidade = 1 }]
        };

        var criarResp = await _client.PostAsJsonAsync("/api/pedidos", pedido, TestContext.Current.CancellationToken);
        var criado = await criarResp.Content.ReadFromJsonAsync<PedidoDto>(TestContext.Current.CancellationToken);

        var patch = new UpdateStatusPedidoDto { NovoStatus = StatusPedido.Pago };
        var response = await _client.PatchAsJsonAsync(
            $"/api/pedidos/{criado!.Id}/status", patch, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var atualizado = await response.Content.ReadFromJsonAsync<PedidoDto>(TestContext.Current.CancellationToken);
        Assert.Equal("Pago", atualizado!.Status);
    }


}
