using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Mapping;

public class VeiculoMap : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Placa, placa =>
        {
            placa.WithOwner();

            placa.Property(p => p.Valor)
                .HasColumnName("Placa")
                .HasMaxLength(8)
                .IsRequired();
        });

        builder.Property(x => x.Modelo)
            .HasMaxLength(100);

        builder.Property(x => x.Tipo)
            .IsRequired();
    }
}