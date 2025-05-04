namespace SecureProject.Shared.DTO
{
    public class ProductFeatureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public ProductDTO Product { get; set; } = default!;
        public int OrderBy { get; set; }

    }
}
