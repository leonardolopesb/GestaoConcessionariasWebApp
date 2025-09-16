using GestaoConcessionariasWebApp.Models.Fabricantes;
using GestaoConcessionariasWebApp.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Fabricante> Fabricantes => Set<Fabricante>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Fabricante>()
             .HasIndex(f => f.NomeFabricante)
             .IsUnique();

            builder.Entity<Fabricante>()
             .HasQueryFilter(f => !f.IsDeleted);

            builder.Entity<Fabricante>()
             .Property(f => f.Website)
             .HasMaxLength(255);
        }
    }
}
