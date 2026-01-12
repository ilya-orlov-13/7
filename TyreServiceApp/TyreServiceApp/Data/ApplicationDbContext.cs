using Microsoft.EntityFrameworkCore;
using TyreServiceApp.Models;

namespace TyreServiceApp.Data
{
    /// <summary>
    /// Контекст базы данных для приложения шиномонтажа.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста базы данных.
        /// </summary>
        /// <param name="options">Параметры конфигурации контекста.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<Tire> Tires { get; set; }
        public DbSet<CompletedWork> CompletedWorks { get; set; }

        /// <summary>
        /// Настраивает модель данных и связи между сущностями.
        /// </summary>
        /// <param name="modelBuilder">Построитель модели.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Cars)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Car)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Master)
                .WithMany(m => m.Orders)
                .HasForeignKey(o => o.MasterId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tire>()
                .HasOne(t => t.Car)
                .WithMany(c => c.Tires)
                .HasForeignKey(t => t.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompletedWork>()
                .HasOne(cw => cw.Order)
                .WithMany(o => o.CompletedWorks)
                .HasForeignKey(cw => cw.OrderNumber)
                .HasPrincipalKey(o => o.OrderNumber)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompletedWork>()
                .HasOne(cw => cw.Service)
                .WithMany(s => s.CompletedWorks)
                .HasForeignKey(cw => cw.ServiceCode)
                .HasPrincipalKey(s => s.ServiceCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompletedWork>()
                .HasOne(cw => cw.Master)
                .WithMany(m => m.CompletedWorks)
                .HasForeignKey(cw => cw.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}