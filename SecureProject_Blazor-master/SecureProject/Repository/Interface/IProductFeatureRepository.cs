using SecureProject.Shared;

namespace SecureProject.Repository.Interface
{
    public interface IProductFeatureRepository
    {
        Task<List<ProductFeature>> GetProductFeaturesAsync();
        Task<ProductFeature?> GetProductFeatureByIdAsync(int id);
        Task AddProductFeatureAsync(ProductFeature product);
        Task<bool> UpdateProductFeatureAsync(ProductFeature product);
        Task<bool> DeleteProductFeatureAsync(int id);
    }
}
