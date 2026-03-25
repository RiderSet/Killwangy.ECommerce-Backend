using FluentValidation;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Validatiors;

public class CriarFaturaCommandValidator : AbstractValidator<CriarFaturaCommand>
{
    public CriarFaturaCommandValidator()
    {
        RuleFor(x => x.Numero).NotEmpty();
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.DataVencimento).GreaterThan(DateTime.MinValue);
        RuleFor(x => x.ValorTotal).GreaterThan(0);
        RuleFor(x => x.IdempotencyKey).NotEmpty();
    }
}