using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Models;

namespace MicroSaldoFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Ingreso> Ingresos => Set<Ingreso>();
        public DbSet<Egreso> Egresos => Set<Egreso>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
