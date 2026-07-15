using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Api.Models;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; private set; }
    public StatusPedido Status { get; private set; }
    public decimal ValorTotal { get; private set; }

    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    private Pedido() { }

    public Pedido(int clienteId)
    {
        ClienteId = clienteId;
        DataPedido = DateTime.UtcNow;
        Status = StatusPedido.Pendente;
        ValorTotal = 0;
    }

    public void AdicionarItem(ItemPedido novoItem)
    {
        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == novoItem.ProdutoId);
        if (itemExistente is not null)
            itemExistente.AdicionarQuantidade(novoItem.Quantidade);
        else
            _itens.Add(novoItem);

        RecalcularValorTotal();
    }

    private void RecalcularValorTotal()
    {
        ValorTotal = _itens.Sum(item => item.CalcularSubtotal());
    }

    public void MudarStatus(StatusPedido novoStatus)
    {
        var transicaoValida = (Status, novoStatus) switch
        {
            (StatusPedido.Pendente, StatusPedido.Pago) => true,
            (StatusPedido.Pendente, StatusPedido.Cancelado) => true,
            (StatusPedido.Pago, StatusPedido.Cancelado) => true,
            _ => false
        };

        if (!transicaoValida)
            throw new InvalidOperationException($"Não é possível mudar o status de {Status} para {novoStatus}");

        Status = novoStatus;
    }
}