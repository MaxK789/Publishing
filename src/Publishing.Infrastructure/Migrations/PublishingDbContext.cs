using Microsoft.EntityFrameworkCore;

namespace Publishing.Infrastructure.Migrations
{
    // EF Core context used solely for applying migrations
    public class PublishingDbContext : DbContext
    {
        public PublishingDbContext(DbContextOptions<PublishingDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonEntity> Persons => Set<PersonEntity>();
        public DbSet<PassEntity> Passes => Set<PassEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrganizationEntity> Organizations => Set<OrganizationEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Person");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("idPerson");
                entity.Property(e => e.FName);
                entity.Property(e => e.LName);
                entity.Property(e => e.Email).HasColumnName("emailPerson");
                entity.Property(e => e.Type).HasColumnName("typePerson");
                entity.Property(e => e.Phone).HasColumnName("phonePerson");
                entity.Property(e => e.Fax).HasColumnName("faxPerson");
                entity.Property(e => e.Address).HasColumnName("addressPerson");
            });

            modelBuilder.Entity<PassEntity>(entity =>
            {
                entity.ToTable("Pass");
                entity.HasKey(e => new { e.PersonId });
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.PersonId).HasColumnName("idPerson");
            });

            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("idProduct");
                entity.Property(e => e.PersonId).HasColumnName("idPerson");
                entity.Property(e => e.Type).HasColumnName("typeProduct");
                entity.Property(e => e.Name).HasColumnName("nameProduct");
                entity.Property(e => e.Pages).HasColumnName("pagesNum");
            });

            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("idOrder");
                entity.Property(e => e.ProductId).HasColumnName("idProduct");
                entity.Property(e => e.PersonId).HasColumnName("idPerson");
                entity.Property(e => e.Printery).HasColumnName("namePrintery");
                entity.Property(e => e.DateOrder).HasColumnName("dateOrder");
                entity.Property(e => e.DateStart).HasColumnName("dateStart");
                entity.Property(e => e.DateFinish).HasColumnName("dateFinish");
                entity.Property(e => e.Status).HasColumnName("statusOrder");
                entity.Property(e => e.Tirage).HasColumnName("tirage");
                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<OrganizationEntity>(entity =>
            {
                entity.ToTable("Organization");
                entity.HasKey(e => e.PersonId);
                entity.Property(e => e.PersonId).HasColumnName("idPerson");
                entity.Property(e => e.Name).HasColumnName("nameOrganization");
                entity.Property(e => e.Email).HasColumnName("emailOrganization");
                entity.Property(e => e.Phone).HasColumnName("phoneOrganization");
                entity.Property(e => e.Fax).HasColumnName("faxOrganization");
                entity.Property(e => e.Address).HasColumnName("addressOrganization");
            });
        }
    }

    public class PersonEntity
    {
        public int Id { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Email { get; set; }
        public string? Type { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
    }

    public class PassEntity
    {
        public string Password { get; set; } = string.Empty;
        public int PersonId { get; set; }
    }

    public class ProductEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public int Pages { get; set; }
    }

    public class OrderEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PersonId { get; set; }
        public string? Printery { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public string? Status { get; set; }
        public int Tirage { get; set; }
        public decimal Price { get; set; }
    }

    public class OrganizationEntity
    {
        public int PersonId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
    }
}
