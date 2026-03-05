using GBastos.Casa_dos_Farelos.PedidoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PedidoService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.PedidoService.Infrastructure.Persistence.Context;

public sealed class PedidoDbContext : DbContext
{
    public PedidoDbContext(DbContextOptions<PedidoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<CarrinhoItem> CarrinhoItens => Set<CarrinhoItem>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automaticamente todas as configurações do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PedidoDbContext).Assembly);

        //modelBuilder.Entity<Pedido>()
        //    .Property(p => p.Status)
        //    .HasConversion<string>();

        modelBuilder.Entity<Carrinho>(builder =>
        {
            builder.ToTable("Carrinhos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .ValueGeneratedNever();

            builder.Property(c => c.ClienteId)
                   .IsRequired();

            builder.Property(c => c.CriadoEm)
                   .IsRequired();

            // Total é calculado, então não deve ser persistido
            builder.Ignore(c => c.Total);

            builder.HasMany<CarrinhoItem>("_itens")
                   .WithOne()
                   .HasForeignKey("CarrinhoId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation("_itens")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<CarrinhoItem>(builder =>
        {
            builder.ToTable("CarrinhoItens");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                   .ValueGeneratedNever();

            builder.Property(ci => ci.ProdutoId)
                   .IsRequired();

            builder.Property(ci => ci.Nome)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(ci => ci.PrecoUnitario)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(ci => ci.Quantidade)
                   .IsRequired();
        });

        modelBuilder.Entity<ItemPedido>(builder =>
        {
            builder.ToTable("ItensPedido");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                   .ValueGeneratedNever();

            builder.Property(i => i.PedidoId)
                   .IsRequired();

            builder.Property(i => i.ProdutoId)
                   .IsRequired();

            builder.Property(i => i.NomeProduto)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(i => i.PrecoUnitario)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(i => i.Quantidade)
                   .IsRequired();

            // SubTotal é calculado
            builder.Ignore(i => i.SubTotal);
        });
    }
}