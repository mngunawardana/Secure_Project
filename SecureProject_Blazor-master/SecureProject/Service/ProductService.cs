using SecureProject.Interface.Service;
using SecureProject.Repository.Interface;
using SecureProject.Shared;

namespace SecureProject.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository) => _repository = repository;
        public async Task<List<Product>> GetProductsAsync() => await _repository.GetProductsAsync();
        public async Task<Product?> GetProductByIdAsync(int id) => await _repository.GetProductByIdAsync(id);
        public async Task AddProductAsync(Product product) => await _repository.AddProductAsync(product);
        public async Task<bool> UpdateProductAsync(Product product) => await _repository.UpdateProductAsync(product);
        public async Task<bool> DeleteProductAsync(int id) => await _repository.DeleteProductAsync(id);
    }
}
