using SecureProject.Shared;
using SecureProject.Shared.DTO;

namespace SecureProject.Interface.Service
{
    public interface IProductFeatureService
    {
        Task<List<ProductFeature >> GetProductFeaturesAsync();
        Task<ProductFeature?> GetProductFeatureByIdAsync(int id);
        Task AddProductFeatureAsync(ProductFeatureAddEditDTO product);
        Task<bool> UpdateProductFeatureAsync(ProductFeatureAddEditDTO product);
        Task<bool> DeleteProductFeatureAsync(int id);
    }
}
