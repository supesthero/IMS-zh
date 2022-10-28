using IMS.CoreBusiness;
using IMS.PlugIns.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task AddProductAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Products.Add(product);
            FlagInventoryUnchanged(product, db);
            await db.SaveChangesAsync();
        }

        private void FlagInventoryUnchanged(Product product, IMSContext db)
        {
            //Because Product has ProductInventories which has Inventory, EFCore will try
            //to add all of the Inventory items. But we don't want that because they are already
            //in the database. So, this foreach loop will stop EFCore from saving the Inventory items
            //and only save to the Products table and ProductInventories table.
            if (product?.ProductInventories != null &&
                product.ProductInventories.Count > 0)
            {
                foreach (var prodInv in product.ProductInventories)
                {
                    if (prodInv.Inventory != null)
                    {
                        db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                    }
                }
            }
        }

        public async Task<Product?> GetProductsByIdAsync(int productId)
        {
            using var db = this.contextFactory.CreateDbContext();
            return await db.Products.Include(x=>x.ProductInventories)
                .ThenInclude(x=>x.Inventory).FirstOrDefaultAsync(x => x.ProductId == productId);   
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var db = this.contextFactory.CreateDbContext();
            return await db.Products.Where(x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();     
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();
            var prod = await db.Products.Include(x => x.ProductInventories)
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
            if(prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;
                prod.ProductInventories = product.ProductInventories;
                FlagInventoryUnchanged(product, db);
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();
            return (await db.Products.Where(x => x.ProductName.ToLower().IndexOf(product.ProductName.ToLower()) >= 0).ToListAsync()).Count > 0;
        }
    }
}