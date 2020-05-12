using CensoDemografico.Domain.Entities;
using CensoDemografico.Infra.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CensoDemografico.Infra.Context
{
    public class CensoDemograficoContext : DbContext
    {
        public CensoDemograficoContext(DbContextOptions<CensoDemograficoContext> options)
            :base(options)
        {

        }

        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaConfiguration());
        }
    }
}
