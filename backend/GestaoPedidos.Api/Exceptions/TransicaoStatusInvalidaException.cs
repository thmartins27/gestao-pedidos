namespace GestaoPedidos.Api.Exceptions;

public class TransicaoStatusInvalidaException : DomainException
{
    public TransicaoStatusInvalidaException(string statusAtual, string statusDestino)
        : base($"Não é possível mudar o status de {statusAtual} para {statusDestino}.") { }
}