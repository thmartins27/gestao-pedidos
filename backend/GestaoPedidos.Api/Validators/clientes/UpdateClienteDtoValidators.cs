using FluentValidation;
using GestaoPedidos.Api.DTOs.Clientes;

namespace GestaoPedidos.Api.Validators.Clientes;

public class UpdateClienteDtoValidators: AbstractValidator<UpdateClienteDto>
{
    public UpdateClienteDtoValidators()
    {
        RuleFor(p => p.Nome)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("o nome é obrigatório")
            .MaximumLength(70).WithMessage("O nome deve ter no máximo 70 caracteres.");

        RuleFor(p => p.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("O e-mail informado não é válido.");
    }
}