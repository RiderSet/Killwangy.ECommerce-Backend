using FluentValidation;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Clientes.ObterCliente;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Validators;

public class ObterClienteQueryValidator : AbstractValidator<ObterClienteQuery>
{
    public ObterClienteQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ClienteId é obrigatório.");
    }
}