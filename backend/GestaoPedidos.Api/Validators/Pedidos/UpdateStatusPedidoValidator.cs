using FluentValidation;
using GestaoPedidos.Api.DTOs.Pedidos;

namespace GestaoPedidos.Api.Validators.Pedidos;

public class UpdateStatusPedidoDtoValidator : AbstractValidator<UpdateStatusPedidoDto>
{
    public UpdateStatusPedidoDtoValidator()
    {
        RuleFor(x => x.NovoStatus)
            .IsInEnum()
            .WithMessage("Status inválido.");
    }
}