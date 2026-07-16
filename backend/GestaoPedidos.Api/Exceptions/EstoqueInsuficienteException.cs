namespace GestaoPedidos.Api.Exceptions;

public class EstoqueInsuficienteException: DomainException
{
    public EstoqueInsuficienteException(string produto, int disponivel, int solicitado) 
        : base($"Estoque insuficiente para o produto '{produto}', " +
               $"Disponível: {disponivel}, solicitado: {solicitado}") {}
}