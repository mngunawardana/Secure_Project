namespace SecureProject.Shared.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ProductFeatureDTO> ProductFeatures { get; set; } = new();
    }
}
