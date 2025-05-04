using Microsoft.EntityFrameworkCore;
using SecureProject.Data;
using SecureProject.Repository.Interface;
using SecureProject.Shared;

namespace SecureProject.B1.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<Product>> GetProductsAsync() =>
            await _context.Products.Include(p => p.ProductFeatures).ToListAsync();

        public async Task<Product?> GetProductByIdAsync(int id) =>
            await _context.Products.Include(p => p.ProductFeatures).FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products
                .Include(p => p.ProductFeatures)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null) return false;

            // Update basic fields
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.VendorLogo = product.VendorLogo;
            existingProduct.VendorName = product.VendorName;
            existingProduct.Price = product.Price;

            // Remove deleted features
            _context.Features.RemoveRange(existingProduct.ProductFeatures.Where(f =>
                !product.ProductFeatures.Any(pf => pf.Id == f.Id)));

            // Update or add features
            foreach (var feature in product.ProductFeatures)
            {
                var existingFeature = existingProduct.ProductFeatures.FirstOrDefault(f => f.Id == feature.Id);
                if (existingFeature != null)
                {
                    existingFeature.Name = feature.Name;
                    existingFeature.Value = feature.Value;
                }
                else
                {
                    existingProduct.ProductFeatures.Add(new ProductFeature
                    {
                        Name = feature.Name,
                        Value = feature.Value
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.Include(p => p.ProductFeatures).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            _context.Features.RemoveRange(product.ProductFeatures);
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
