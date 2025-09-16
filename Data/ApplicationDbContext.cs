using GestaoConcessionariasWebApp.Models.Fabricantes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Fabricante> Fabricantes => Set<Fabricante>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Fabricante>()
             .HasIndex(f => f.Nome)
             .IsUnique();

            b.Entity<Fabricante>()
             .HasQueryFilter(f => !f.IsDeleted);

            b.Entity<Fabricante>()
             .Property(f => f.Website)
             .HasMaxLength(255);
        }
    }
}
