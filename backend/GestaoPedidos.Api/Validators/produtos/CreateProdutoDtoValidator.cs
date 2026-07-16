using FluentValidation;
using GestaoPedidos.Api.DTOs.Produtos;

namespace GestaoPedidos.Api.Validators.Produtos;

public class CreateProdutoDtoValidator: AbstractValidator<CreateProdutoDto>
{
    public CreateProdutoDtoValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");
        
        RuleFor(p => p.Preco)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero");

        RuleFor(p => p.EstoqueAtual)
            .GreaterThan(0).WithMessage("O estoque deve ser maior que o zero");
    }
}