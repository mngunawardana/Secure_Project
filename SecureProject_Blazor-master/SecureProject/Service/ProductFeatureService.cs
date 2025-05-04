using SecureProject.Interface.Service;
using SecureProject.Repository.Interface;
using SecureProject.Shared;
using SecureProject.Shared.DTO;

namespace SecureProject.Service
{
    public class ProductFeatureService : IProductFeatureService
    {
        private readonly IProductFeatureRepository _repository;
        public ProductFeatureService(IProductFeatureRepository repository) => _repository = repository;
        public async Task<List<ProductFeature>> GetProductFeaturesAsync() => await _repository.GetProductFeaturesAsync();
        public async Task<ProductFeature?> GetProductFeatureByIdAsync(int id) => await _repository.GetProductFeatureByIdAsync(id);
        public async Task AddProductFeatureAsync(ProductFeatureAddEditDTO feature)
        {
            var featureDbModel = new ProductFeature
            {
                Name = feature.Name,
                Value = feature.Value,
                ProductId = feature.ProductId,
                OrderBy = feature.OrderBy
            };
            await _repository.AddProductFeatureAsync(featureDbModel);
        }
        public async Task<bool> UpdateProductFeatureAsync(ProductFeatureAddEditDTO feature)
        {
            var existingProduct = await _repository.GetProductFeatureByIdAsync(feature.Id);
            existingProduct.Value = feature.Value;
            existingProduct.Name = feature.Name;
            existingProduct.OrderBy = feature.OrderBy;
            return await _repository.UpdateProductFeatureAsync(existingProduct);
        }
        public async Task<bool> DeleteProductFeatureAsync(int id) => await _repository.DeleteProductFeatureAsync(id);
    }
}
