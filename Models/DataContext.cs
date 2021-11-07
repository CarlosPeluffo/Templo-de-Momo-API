using System;
using Microsoft.EntityFrameworkCore;

namespace Templo_de_Momo.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Biblioteca>()
                .HasKey(b => new { b.UsuarioId, b.JuegoId });
        }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Creador> creadores { get; set; }
        public DbSet<Juego> juegos { get; set; }
        public DbSet<Noticia> noticias { get; set; }
        public DbSet<Comentario> comentarios { get; set; }
        public DbSet<Biblioteca> bibliotecas { get; set; }
    }
}