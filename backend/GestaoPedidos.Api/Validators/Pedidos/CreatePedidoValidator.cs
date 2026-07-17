using FluentValidation;
using GestaoPedidos.Api.DTOs.Pedidos;

namespace GestaoPedidos.Api.Validators.Pedidos;

public class CreatePedidoValidator : AbstractValidator<CreatePedidoDto>
{
    public CreatePedidoValidator()
    {
        RuleFor(x => x.ClienteId)
            .GreaterThan(0).WithMessage("O Cliente é obrigatório");

        RuleFor(x => x.Itens)
            .NotEmpty().WithMessage("O pedido deve conter ao menos um item.");

        RuleForEach(x => x.Itens).ChildRules(item =>
        {
            item.RuleFor(i => i.ProdutoId)
             .GreaterThan(0).WithMessage("Produto inválido no item do pedido.");

            item.RuleFor(i => i.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
        });
    }
}