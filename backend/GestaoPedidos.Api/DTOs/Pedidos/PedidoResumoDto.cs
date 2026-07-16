namespace GestaoPedidos.Api.DTOs.Pedidos;

public class PedidoResumoDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
}