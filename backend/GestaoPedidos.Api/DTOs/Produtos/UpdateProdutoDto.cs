namespace GestaoPedidos.Api.DTOs.Produtos;

public class UpdateProdutoDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int EstoqueAtual { get; set; }
}