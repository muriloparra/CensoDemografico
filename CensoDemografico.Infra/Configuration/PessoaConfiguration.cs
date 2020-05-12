using CensoDemografico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CensoDemografico.Infra.Configuration
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(x => x.PessoaId);
            builder.Property(x => x.Nome).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Sobrenome).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Etnia);
            builder.Property(x => x.Genero);
            builder.Property(x => x.Escolaridade);

            builder.HasOne(x => x.Pai)
                .WithMany()
                .HasForeignKey(x => x.IdPai)
                .HasPrincipalKey(x => x.PessoaId);

            builder.HasOne(x => x.Mae)
                .WithMany()
                .HasForeignKey(x => x.IdMae)
                .HasPrincipalKey(x => x.PessoaId);
        }
    }
}