namespace GestaoPedidos.Api.DTOs.Produtos;

public class CreateProdutoDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int EstoqueAtual { get; set; }
}