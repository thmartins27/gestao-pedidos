namespace GestaoPedidos.Api.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string mensagem) : base(mensagem) {}
}