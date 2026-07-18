using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Api.DTOs.Pedidos;

public class DashboardPedidoDto
{
    public int TotalPedidos { get; set; }
    public decimal ValorTotalPedidos { get; set; }
    public List<PedidoStatusDto> PedidosPorStatus { get; set; } = [];
}

public class PedidoStatusDto
{
    public StatusPedido Status { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorTotal { get; set; }
}