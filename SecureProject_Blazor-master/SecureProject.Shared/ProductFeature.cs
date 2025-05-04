namespace SecureProject.Shared
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int OrderBy { get; set; }
        //public Product Product { get; set; } = default!;
    }
}
