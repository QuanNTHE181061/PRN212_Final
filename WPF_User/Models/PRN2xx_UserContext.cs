using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace WPF_User.Models
{
    public partial class PRN2xx_UserContext : DbContext
    {
        public PRN2xx_UserContext()
        {
        }

        public PRN2xx_UserContext(DbContextOptions<PRN2xx_UserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Hobby> Hobbies { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Person> Persons { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Street).HasMaxLength(100);

                entity.Property(e => e.ZipCode).HasMaxLength(10);
            });

            modelBuilder.Entity<Hobby>(entity =>
            {
                entity.Property(e => e.HobbyId).HasColumnName("HobbyID");

                entity.Property(e => e.HobbyName).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK__Order__PersonID__29572725");

                entity.HasMany(d => d.Products)
                    .WithMany(p => p.Orders)
                    .UsingEntity<Dictionary<string, object>>(
                        "OrderProduct",
                        l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__OrderProd__Produ__3F466844"),
                        r => r.HasOne<Order>().WithMany().HasForeignKey("OrderId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__OrderProd__Order__3E52440B"),
                        j =>
                        {
                            j.HasKey("OrderId", "ProductId").HasName("PK__OrderPro__08D097C1E321E83C");

                            j.ToTable("OrderProduct");

                            j.IndexerProperty<int>("OrderId").HasColumnName("OrderID");

                            j.IndexerProperty<int>("ProductId").HasColumnName("ProductID");
                        });
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK__Person__AddressI__267ABA7A");

                entity.HasMany(d => d.Hobbies)
                    .WithMany(p => p.People)
                    .UsingEntity<Dictionary<string, object>>(
                        "PersonHobby",
                        l => l.HasOne<Hobby>().WithMany().HasForeignKey("HobbyId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__PersonHob__Hobby__4CA06362"),
                        r => r.HasOne<Person>().WithMany().HasForeignKey("PersonId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__PersonHob__Perso__4BAC3F29"),
                        j =>
                        {
                            j.HasKey("PersonId", "HobbyId").HasName("PK__PersonHo__4A841B3B75AF33BE");

                            j.ToTable("PersonHobbies");

                            j.IndexerProperty<int>("PersonId").HasColumnName("PersonID");

                            j.IndexerProperty<int>("HobbyId").HasColumnName("HobbyID");
                        });
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
