using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;

public class CadastroDbContext : DbContext
{
    public CadastroDbContext(
        DbContextOptions<CadastroDbContext> options)
        : base(options)
    {
    }

    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Funcionario>().HasKey(x => x.Id);
        builder.Entity<Veiculo>().HasKey(x => x.Id);
        builder.Entity<Fornecedor>().HasKey(x => x.Id);
        builder.Entity<Cliente>().HasKey(x => x.Id);


        builder.ApplyConfiguration(new VeiculoMap());
    }
}