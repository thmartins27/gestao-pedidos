using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Api.DTOs.Pedidos;

public class UpdateStatusPedidoDto
{
    public StatusPedido NovoStatus { get; set; }
}