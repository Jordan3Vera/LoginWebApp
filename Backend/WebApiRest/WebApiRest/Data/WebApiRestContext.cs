using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using WebApiRest.Model;

namespace WebApiRest.Data
{
    public class WebApiRestContext : DbContext
    {
        #region Explicaciones 
        //private readonly string _connectionString;

        //public WebApiRestContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}
        //También facilita el paso de la configuración como la cadena de conexión a través del DbContext
        //optionsBuilder.UseSqlServer(_connectionString);
        #endregion

        //static LoggerFactory object
        //public static readonly ILoggerFactory loggerFactory = new LoggerFactory(
        //      new[] { new ConsoleLoggerProvider((_, __) => true, true) }
        //);
        //o 
        //public static readonly ILoggerFactory loggerFactory = new LoggerFactory().AddConsole((_, __) => true);

        /*Una instancia DbContext está diseñada para ser utilizada para una sola unidad de trabajo*/
        //No comparte contexto entre subprocesos. es necesario hacer todas las llamadas asincrónicas
        public WebApiRestContext (DbContextOptions<WebApiRestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Product> Product { get; set; }

        #region Esto permite seleccionar y configurar la fuente de datos a utilizar con un contexto mediante DbContextOptionsBuilder
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var db = @"Server=(localdb)\\Servidor; Database=BD_TEST;Integrated Security=True;Encrypt=true;Trusted_Connection=true";
        //    optionsBuilder.UseLoggerFactory(loggerFactory)
        //        .EnableSensitiveDataLogging()
        //        .UseSqlServer(db);
        //}
        #endregion


        //Esto permite configurar el modelo usando ModelBuilder fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(32)
                .IsUnicode(false);
                entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(32)
                .IsUnicode(false);
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(32)
                .IsUnicode(false);
                entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(32)
                .IsUnicode(false);
            });
            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Date).HasColumnType("smalldatetime");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");
                entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusId");

                entity.HasOne(d => d.Customer)
                .WithMany(p => p.CustomerOrder)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Customer_CustomerOrder");

                entity.HasOne(d => d.OrderStatus)
                .WithMany(p => p.CustomerOrder)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatus_CustomerOrder");
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.CustomerOrderId, e.ProductId });
                entity.Property(e => e.CustomerOrderId).HasColumnName("CustomerOrderId");
                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
                entity.HasOne(d => d.CustomerOrder)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.CustomerOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerOrder_OrderDetail");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Product_OrderDetail");
            });
            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(255)
                   .IsUnicode(false);
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(8,2)");
            });
        }
    }
}
