using System.Net;
using System.Net.Http.Json;
using GestaoPedidos.Api.DTOs.Produtos;

namespace GestaoPedidos.Tests.Integration;

[Collection("Integration")]
public class ProdutoIntegrationTests
{
    private readonly HttpClient _client;

    public ProdutoIntegrationTests (IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CriarProduto_ComDadosValidos_RetornaCreated()
    {
        var novoProduto = new CreateProdutoDto
        {
            Nome = "Webcam Full HD",
            Preco = 250.00m,
            EstoqueAtual = 15
        };

        var response = await _client.PostAsJsonAsync("/api/produtos", novoProduto, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);


        var criado = await response.Content.ReadFromJsonAsync<ProdutoDto>(
            TestContext.Current.CancellationToken
        );

        Assert.NotNull(criado);
        Assert.Equal("Webcam Full HD", criado!.Nome);
        Assert.True(criado.Id > 0);
    }
}