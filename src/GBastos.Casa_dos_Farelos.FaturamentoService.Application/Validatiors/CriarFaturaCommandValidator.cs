namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Validatiors;

public class CriarFaturaCommandValidator
    : AbstractValidator<CriarFaturaCommand>
{
    public CriarFaturaCommandValidator()
    {
        RuleFor(x => x.Numero)
            .NotEmpty()
            .WithMessage("Número da fatura é obrigatório.")
            .MaximumLength(50);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .WithMessage("IdempotencyKey é obrigatória.")
            .MaximumLength(100);
    }
}