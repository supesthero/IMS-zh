using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace IMS.PlugIns.EFCoreSqlServer
{
    public class IMSContext : DbContext
    {
        public IMSContext(DbContextOptions<IMSContext> options) : base(options)
        {

        }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ProductTransaction> ProductTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInventory>()
                .HasKey(x => new { x.ProductId, x.InventoryId });
            modelBuilder.Entity<ProductInventory>()
                .HasOne(x => x.Product)
                .WithMany(x => x.ProductInventories)
                .HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<ProductInventory>()
                .HasOne(x => x.Inventory)
                .WithMany(x => x.ProductInventories)
                .HasForeignKey(x => x.InventoryId);

            //seeding data
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory { InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2 },
                new Inventory { InventoryId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15 },
                new Inventory { InventoryId = 3, InventoryName = "Bike Wheel", Quantity = 20, Price = 8 },
                new Inventory { InventoryId = 4, InventoryName = "Bike Pedal", Quantity = 20, Price = 1 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product() { ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 150 },
                new Product() { ProductId = 2, ProductName = "Car", Quantity = 5, Price = 25000 }
                );

            modelBuilder.Entity<ProductInventory>().HasData(
                new ProductInventory { InventoryId = 1, ProductId = 1, InventoryQuantity = 1 },
                new ProductInventory { InventoryId = 2, ProductId = 1, InventoryQuantity = 1 },
                new ProductInventory { InventoryId = 3, ProductId = 1, InventoryQuantity = 2 },
                new ProductInventory { InventoryId = 4, ProductId = 1, InventoryQuantity = 2 }
                );
        }
    }
}