using Microsoft.EntityFrameworkCore;
using Publishing.Infrastructure.Entities;

namespace Publishing.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons => Set<Person>();
        public DbSet<Pass> Passes => Set<Pass>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("idPerson");
                entity.Property(e => e.FName).HasColumnName("FName");
                entity.Property(e => e.LName).HasColumnName("LName");
                entity.Property(e => e.Email).HasColumnName("emailPerson");
                entity.Property(e => e.Type).HasColumnName("typePerson");
                entity.Property(e => e.Phone).HasColumnName("phonePerson");
                entity.Property(e => e.Fax).HasColumnName("faxPerson");
                entity.Property(e => e.Address).HasColumnName("addressPerson");
            });

            modelBuilder.Entity<Pass>(entity =>
            {
                entity.ToTable("Pass");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.PersonId).HasColumnName("idPerson");
                entity.HasOne(e => e.Person)
                      .WithOne(p => p.Pass!)
                      .HasForeignKey<Pass>(e => e.PersonId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("idProduct");
                entity.Property(e => e.PersonId).HasColumnName("idPerson");
                entity.Property(e => e.Type).HasColumnName("typeProduct");
                entity.Property(e => e.Name).HasColumnName("nameProduct");
                entity.Property(e => e.Pages).HasColumnName("pagesNum");
                entity.HasOne(e => e.Person)
                      .WithMany(p => p.Products)
                      .HasForeignKey(e => e.PersonId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("idOrder");
                entity.Property(e => e.ProductId)
                      .HasColumnName("idProduct")
                      .IsRequired(false);
                entity.Property(e => e.PersonId).HasColumnName("idPerson");
                entity.Property(e => e.NamePrintery).HasColumnName("namePrintery");
                entity.Property(e => e.DateOrder).HasColumnName("dateOrder");
                entity.Property(e => e.DateStart).HasColumnName("dateStart");
                entity.Property(e => e.DateFinish).HasColumnName("dateFinish");
                entity.Property(e => e.Status).HasColumnName("statusOrder");
                entity.Property(e => e.Tirage).HasColumnName("tirage");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.HasOne(e => e.Product)
                      .WithMany(p => p.Orders)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Person)
                      .WithMany(p => p.Orders)
                      .HasForeignKey(e => e.PersonId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
