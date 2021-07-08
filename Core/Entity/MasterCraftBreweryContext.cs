using Core.Entity.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Entity
{
    public class MasterCraftBreweryContext : DbContext
    {
        public MasterCraftBreweryContext(DbContextOptions<MasterCraftBreweryContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<SocialMedia> SocialMedias { get; set; }

        public DbSet<Wholesale> Wholesales { get; set; }

        public DbSet<Serving> Servings { get; set; }

        public DbSet<ProductServing> ProductServings { get; set; }

        public DbSet<ShopAmount> ShopAmounts { get; set; }

        public DbSet<ShopProductServing> ShopProductServings { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<Administrator> Administrators { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Gallery> Galleries { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<MediaFile> MediaFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new MenuConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServingConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductServingConfiguration());
            modelBuilder.ApplyConfiguration(new MenuItemConfiguration());
            modelBuilder.ApplyConfiguration(new SocialMediaConfiguration());
            modelBuilder.ApplyConfiguration(new WholesaleConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());
            modelBuilder.ApplyConfiguration(new AdministratorConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientConfiguration());
            modelBuilder.ApplyConfiguration(new GalleryConfiguration());
            modelBuilder.ApplyConfiguration(new QuoteConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new MediaFileConfiguration());
            modelBuilder.ApplyConfiguration(new ShopAmountConfiguration());
            modelBuilder.ApplyConfiguration(new ShopProductServingConfiguration());
            modelBuilder.ApplyConfiguration(new ProductOrderConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
