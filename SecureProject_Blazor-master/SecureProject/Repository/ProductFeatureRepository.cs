using Microsoft.EntityFrameworkCore;
using SecureProject.Data;
using SecureProject.Repository.Interface;
using SecureProject.Shared;

namespace SecureProject.B1.Repository
{
    public class ProductFeatureRepository : IProductFeatureRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductFeatureRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<ProductFeature>> GetProductFeaturesAsync() =>
            await _context.Features.ToListAsync();

        public async Task<ProductFeature?> GetProductFeatureByIdAsync(int id) =>
            await _context.Features.FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddProductFeatureAsync(ProductFeature product)
        {
            await _context.Features.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductFeatureAsync(ProductFeature product)
        {
            var existingProductFeature = await _context.Features
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProductFeature == null) return false;

            // Update basic fields
            existingProductFeature.Name = product.Name;
            existingProductFeature.Value = product.Value;

         

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductFeatureAsync(int id)
        {
            var product = await _context.Features.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            _context.Features.RemoveRange(product);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
