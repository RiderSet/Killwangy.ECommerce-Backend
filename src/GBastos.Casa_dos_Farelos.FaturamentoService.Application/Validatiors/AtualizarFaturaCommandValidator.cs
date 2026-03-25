using FluentValidation;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Validatiors;

public class AtualizarFaturaCommandValidator : AbstractValidator<AtualizarFaturaCommand>
{
    public AtualizarFaturaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DataVencimento).GreaterThan(DateTime.MinValue);
        RuleFor(x => x.ValorTotal).GreaterThan(0);
        RuleFor(x => x.IdempotencyKey).NotEmpty();
    }
}