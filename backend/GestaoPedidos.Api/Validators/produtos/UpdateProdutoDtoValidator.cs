using FluentValidation;
using GestaoPedidos.Api.DTOs.Produtos;

namespace GestaoPedidos.Api.Validators.Produtos;

public class UpdateProdutoDtoValidator: AbstractValidator<CreateProdutoDto>
{
    public UpdateProdutoDtoValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");
        
        RuleFor(p => p.Preco)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero");
    }
}