using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public class Produto : AggregateRoot<Guid>
{
    protected Produto() : base(Guid.Empty) { }

    public Produto(Guid id) : base(id)
    {
        if (id == Guid.Empty)
            throw new DomainException("Produto inválido.");
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Produto inválido.");
    }
}