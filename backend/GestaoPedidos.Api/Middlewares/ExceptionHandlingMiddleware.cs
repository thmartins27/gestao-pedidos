using System.Net;
using System.Text.Json;
using GestaoPedidos.Api.Exceptions;

namespace GestaoPedidos.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }catch (Exception ex)
        {
            await TratarExcecaoAsync(context, ex);
        }
    }

    private async Task TratarExcecaoAsync(HttpContext context, Exception exception)
    {
        var (status, titulo) = exception switch
        {   RecursoNaoEncontradoException => (HttpStatusCode.NotFound, "Recurso não encontrado"),
            EstoqueInsuficienteException => (HttpStatusCode.BadRequest, "Regra de negócio violada"),
            DomainException => (HttpStatusCode.BadRequest, "Regra de negócio violada"),
            ArgumentException => (HttpStatusCode.BadRequest, "Argumento Inválido"),
            _ => (HttpStatusCode.InternalServerError, "Erro interno do servidor")
        };

        if (status == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Erro não tratado: {Mensagem}", exception.Message);
        else
            _logger.LogWarning("Erro de negócio: {Mensagem}", exception.Message);
        

        var problema = new
        {
            status = (int)status,
            title = titulo,
            detail = status == HttpStatusCode.InternalServerError
                ? "Ocorreu um erro inesperado. Tente novamente"
                : exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var json = JsonSerializer.Serialize(problema, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        await context.Response.WriteAsync(json);
    }

}