using BankSystem.API.model;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.API.data
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; } 
        public DbSet<Conta> Contas { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //
            modelBuilder.Entity<Conta>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.NumeroConta)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.HasIndex(e => e.NumeroConta)
                      .IsUnique();

              
                entity.Property(e => e.Saldo)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DataCriacao)
                      .HasDefaultValueSql("GETDATE()");

                
                entity.Property(e => e.Tipo).HasConversion<string>().HasMaxLength(50);
                entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
            });

            
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nome).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Documento).IsRequired().HasMaxLength(20);
                entity.HasIndex(c => c.Documento).IsUnique();
                entity.Property(c => c.DataCriacao).HasDefaultValueSql("GETDATE()");
            });

            
            modelBuilder.Entity<Conta>()
                .HasOne(conta => conta.Client)
                .WithMany(cliente => cliente.Contas)
                .HasForeignKey(conta => conta.ClientId) // Agora a FK ClientId existe na classe
                .OnDelete(DeleteBehavior.Restrict); // Impede de deletar um cliente que tem contas
        }
    }
}