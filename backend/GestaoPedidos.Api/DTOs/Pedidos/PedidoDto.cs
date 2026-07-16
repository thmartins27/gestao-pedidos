namespace GestaoPedidos.Api.DTOs.Pedidos;

public class PedidoDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = [];
}