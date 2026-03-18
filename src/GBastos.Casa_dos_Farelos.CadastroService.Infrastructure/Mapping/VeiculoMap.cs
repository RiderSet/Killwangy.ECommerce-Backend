using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Mapping;

public class VeiculoMap : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.ToTable("Veiculos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Modelo)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(v => v.Tipo)
            .IsRequired();

        builder.Property(v => v.ProprietarioId)
            .IsRequired(false);

        // Value Object Placa
        builder.OwnsOne(v => v.Placa, placa =>
        {
            placa.Property(p => p.Valor)
                .HasColumnName("Placa")
                .HasMaxLength(8)
                .IsRequired();

            placa.HasIndex(p => p.Valor)
                .IsUnique();
        });
    }
}