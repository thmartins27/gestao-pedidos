using System.Text.Json.Serialization;

namespace GestaoPedidos.Api.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusPedido
{
    Pendente,
    Pago,
    Cancelado
}