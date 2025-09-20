using GestaoConcessionariasWebApp.Models.Clientes;
using GestaoConcessionariasWebApp.Models.Concessionarias;
using GestaoConcessionariasWebApp.Models.Fabricantes;
using GestaoConcessionariasWebApp.Models.Users;
using GestaoConcessionariasWebApp.Models.Veiculos;
using GestaoConcessionariasWebApp.Models.Vendas;
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
        public DbSet<Venda> Vendas => Set<Venda>();
        public DbSet<Cliente> Clientes => Set<Cliente>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configurações base
            base.OnModelCreating(builder);

            // Configurações do Usuário
            builder.Entity<ApplicationUser>(e =>
            {
                e.Property(u => u.NomeUsuario).HasMaxLength(50);
                e.Property(u => u.AccessLevel).HasConversion<string>();
                e.Property(u => u.Email).HasMaxLength(100);
                e.Property(u => u.UserName).HasMaxLength(50);

                e.HasQueryFilter(u => !u.IsDeleted);
            });


            // Configurações do Fabricante
            builder.Entity<Fabricante>()
                .HasIndex(f => f.NomeFabricante)
                .IsUnique();

            builder.Entity<Fabricante>()
                .HasQueryFilter(f => !f.IsDeleted);

            builder.Entity<Fabricante>()
                .Property(f => f.Website)
                .HasMaxLength(255);

            // Configurações da Concessionária
            builder.Entity<Concessionaria>()
                .HasIndex(c => c.Nome)
                .IsUnique();

            builder.Entity<Concessionaria>()
                .HasQueryFilter(c => !c.IsDeleted);

            // Configurações do Cliente
            builder.Entity<Cliente>()
                .HasIndex(c => c.CPF)
                .IsUnique();

            builder.Entity<Cliente>()
                .HasQueryFilter(c => !c.IsDeleted);

            // Configurações da Venda
            builder.Entity<Venda>()
                .HasIndex(v => v.ProtocoloVenda)
                .IsUnique();

            builder.Entity<Venda>()
                .Property(v => v.PrecoVenda)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Venda>()
                .Property(v => v.ProtocoloVenda)
                .HasMaxLength(20)
                .IsRequired();

            builder.Entity<Venda>()
                .HasQueryFilter(v => !v.IsDeleted);

            // Configurações do Veículo
            builder.Entity<Veiculo>()
                .Property(v => v.Preco)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Veiculo>()
                .HasQueryFilter(v => !v.IsDeleted);

            // Relacionamento Veículo N:1 Fabricante
            builder.Entity<Veiculo>()
                .HasOne(v => v.Fabricante)
                .WithMany()
                .HasForeignKey(v => v.FabricanteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamentos Venda N:1 Veiculo
            builder.Entity<Venda>()
                .HasOne(v => v.Veiculo)
                .WithMany()
                .HasForeignKey(v => v.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamentos Venda N:1 Concessionaria
            builder.Entity<Venda>()
                .HasOne(v => v.Concessionaria)
                .WithMany()
                .HasForeignKey(v => v.ConcessionariaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamentos Venda N:1 Cliente
            builder.Entity<Venda>()
                .HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
