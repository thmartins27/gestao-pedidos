namespace GestaoPedidos.Api.DTOs.Pedidos;

public class CreatePedidoItemDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}