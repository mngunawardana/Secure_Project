namespace SecureProject.Shared
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Decimal Price { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public byte[]? VendorLogo { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; } = new();
    }
}
