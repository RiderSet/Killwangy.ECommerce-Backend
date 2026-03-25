using FluentValidation;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Validatiors;

public class EmitirFaturaCommandValidator
    : AbstractValidator<EmitirFaturaCommand>
{
    public EmitirFaturaCommandValidator()
    {
        RuleFor(x => x.Numero)
            .NotEmpty()
            .WithMessage("Número da fatura é obrigatório.")
            .MaximumLength(50)
            .WithMessage("Número da fatura deve ter no máximo 50 caracteres.");

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .WithMessage("IdempotencyKey é obrigatória.")
            .MaximumLength(100);
    }
}