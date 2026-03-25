using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Persistence.Context;

public class FaturamentoDbContext : DbContext
{
    private readonly IMediator _mediator;

    public FaturamentoDbContext(
        DbContextOptions<FaturamentoDbContext> options,
        IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Fatura> Faturas => Set<Fatura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FaturamentoDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<Fatura>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        domainEntities
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        return result;
    }
}