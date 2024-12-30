using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using EmailSender.Authorization.Roles;
using EmailSender.Authorization.Users;
using EmailSender.MultiTenancy;
using EmailSender.EmailSender.EmailSenderEntities;
using Microsoft.EntityFrameworkCore.Proxies;
using EmailSender.ProductDomain;
using EmailSender.OrderDomain;
using EmailSender.CartEntity;
using EmailSender.ProductEntities;
using EmailSender.CategoryEntity;


namespace EmailSender.EntityFrameworkCore
{
    public class EmailSenderDbContext : AbpZeroDbContext<Tenant, Role, User, EmailSenderDbContext>
    {
        public EmailSenderDbContext(DbContextOptions<EmailSenderDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductDetails)
                .WithOne(pd => pd.Product)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductMedia)
                .WithOne(pm => pm.Product)
                .HasForeignKey(pm => pm.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

    
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductCategories)
                .WithOne(pc => pc.Product)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
            optionsBuilder.UseLazyLoadingProxies(false);
        }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductsCategorys { get; set; }
        public DbSet<ProductMedia> ProductMedias { get; set; }

        public DbSet<ProductDetail> ProductDetails { get; set; }

        public DbSet<Category> Categorys { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<Cart> Carts { get; set; }

    }
}
