using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotelBooking.Api.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace MotelBooking.Api.Data
{
    /// <summary>
    /// Contexto do banco de dados que herda de IdentityDbContext para suporte a autenticação
    /// </summary>
    public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// Construtor que recebe as opções de configuração do contexto
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define as tabelas do banco de dados
        public DbSet<Motel> Moteis { get; set; }
        public DbSet<Quarto> Quartos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        /// <summary>
        /// Configura o modelo de dados usando Fluent API
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Chama a configuração base do Identity
            base.OnModelCreating(builder);

            // Configura todos os campos DateTime para usar UTC com fuso horário
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));
                
                foreach (var property in properties)
                {
                    property.SetColumnType("timestamp with time zone");
                }
            }

            // Configura o relacionamento entre Reserva e Usuario (1:N)
            builder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configura o relacionamento entre Reserva e Quarto (1:N)
            builder.Entity<Reserva>()
                .HasOne(r => r.Quarto)
                .WithMany()
                .HasForeignKey(r => r.QuartoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configura o relacionamento entre Quarto e Motel (1:N)
            builder.Entity<Quarto>()
                .HasOne(q => q.Motel)
                .WithMany()
                .HasForeignKey(q => q.MotelId);

            // Configura geração automática de GUIDs usando função do PostgreSQL
            builder.Entity<Motel>()
                .Property(m => m.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Entity<Quarto>()
                .Property(q => q.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Entity<Reserva>()
                .Property(r => r.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Garante que emails sejam únicos no sistema
            builder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
