using GestaoConcessionariasWebApp.Models.Concessionarias;
using GestaoConcessionariasWebApp.Models.Fabricantes;
using GestaoConcessionariasWebApp.Models.Users;
using GestaoConcessionariasWebApp.Models.Veiculos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Fabricante> Fabricantes => Set<Fabricante>();
        public DbSet<Veiculo> Veiculos => Set<Veiculo>();
        public DbSet<Concessionaria> Concessionarias => Set<Concessionaria>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configurações base
            base.OnModelCreating(builder);

            // Configurações do Fabricante
            builder.Entity<Fabricante>()
             .HasIndex(f => f.NomeFabricante)
             .IsUnique();

            builder.Entity<Fabricante>()
             .HasQueryFilter(f => !f.IsDeleted);

            builder.Entity<Fabricante>()
             .Property(f => f.Website)
             .HasMaxLength(255);

            // Configurações do Veículo
            builder.Entity<Veiculo>()
             .Property(v => v.Preco)
             .HasColumnType("decimal(18,2)");

            builder.Entity<Veiculo>()
             .HasQueryFilter(v => !v.IsDeleted);

            builder.Entity<Veiculo>()
             .HasOne(v => v.Fabricante)
             .WithMany()
             .HasForeignKey(v => v.FabricanteId)
             .OnDelete(DeleteBehavior.Restrict);

            // Configurações da Concessionária
            builder.Entity<Concessionaria>()
              .HasIndex(c => c.Nome)
              .IsUnique();

            builder.Entity<Concessionaria>()
              .HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
