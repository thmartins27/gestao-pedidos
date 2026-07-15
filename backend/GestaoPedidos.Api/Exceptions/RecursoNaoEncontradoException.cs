namespace GestaoPedidos.Api.Exceptions;

public class RecursoNaoEncontradoException : DomainException
{
    public RecursoNaoEncontradoException(string recurso, int id)
        : base($"{recurso} com Id {id} não foi encontrado") { }
}