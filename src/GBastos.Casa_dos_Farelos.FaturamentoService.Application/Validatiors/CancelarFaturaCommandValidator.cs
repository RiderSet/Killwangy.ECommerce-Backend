using FluentValidation;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Validatiors;

public class CancelarFaturaCommandValidator
    : AbstractValidator<CancelarFaturaCommand>
{
    public CancelarFaturaCommandValidator()
    {
        RuleFor(x => x.FaturaId)
            .NotEmpty()
            .WithMessage("Id da fatura é obrigatório.");

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .WithMessage("IdempotencyKey é obrigatória.")
            .MaximumLength(100);
    }
}