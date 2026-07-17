namespace GestaoPedidos.Api.DTOs.Pedidos;

public class CreatePedidoDto
{
    public int ClienteId { get; set; }
    public List<CreatePedidoItemDto> Itens { get; set; } = [];
}