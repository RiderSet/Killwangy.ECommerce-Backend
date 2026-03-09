using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;

public class EstoqueDbContext : DbContext
{
    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options)
        : base(options) { }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<IdempotencyKey> IdempotencyKeys => Set<IdempotencyKey>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            typeof(EstoqueDbContext).Assembly);

        ConfigureProduto(builder);
        ConfigureReserva(builder);
        ConfigureOutbox(builder);
        ConfigureIdempotency(builder);

        base.OnModelCreating(builder);
    }

    private static void ConfigureProduto(ModelBuilder builder)
    {
        builder.Entity<Produto>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.DescricaoProduto)
                .IsRequired()
                .HasMaxLength(500);

            b.Property(x => x.PrecoVenda)
                .HasPrecision(18, 2);

            b.Property(x => x.QuantEstoque)
                .IsRequired();

            b.Property(x => x.RowVersion)
                .IsRowVersion();

            b.HasIndex(x => x.Nome);

            b.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.CategoriaId);
        });
    }

    private static void ConfigureReserva(ModelBuilder builder)
    {
        builder.Entity<Reserva>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.RowVersion)
                .IsRowVersion();

            b.Property(x => x.Quantidade)
                .IsRequired();

            b.HasIndex(x => x.ProdutoId);
            b.HasIndex(x => x.ExpiraEm);
        });
    }

    private static void ConfigureOutbox(ModelBuilder builder)
    {
        builder.Entity<OutboxMessage>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Type)
                .IsRequired();

            b.Property(x => x.Payload)
                .IsRequired();

            b.HasIndex(x => x.ProcessedOnUtc);
            b.HasIndex(x => x.Attempts);
        });
    }

    private static void ConfigureIdempotency(ModelBuilder builder)
    {
        builder.Entity<IdempotencyKey>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Key)
                .IsRequired();

            b.HasIndex(x => x.Key)
                .IsUnique();
        });
    }
}