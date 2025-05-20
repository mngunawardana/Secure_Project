using SecureProject.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace SecureProject.Client.Service
{
    public class ProductRecommendationService
    {
        private readonly HttpClient _httpClient;

        public ProductRecommendationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.openai.com/");
            var apiKey = "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"; // Store this in appsettings.json or secret manager
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<List<Product>> GetRecommendedProductsAsync(string requirement, List<Product> allProducts)
        {
            var prompt = GeneratePrompt(requirement, allProducts);

            var requestBody = new
            {
                //model = "gpt-3.5-turbo",
                model = "gpt-4.1",
                messages = new[]
                {
                new { role = "system", content = "You are a product recommendation expert." },
                new { role = "user", content = prompt }
            },
                max_tokens = 1000,
                temperature = 0.7
            };

            var response = await _httpClient.PostAsJsonAsync("v1/chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API error: {error}");
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            // You can now deserialize the result and extract recommended product names
            // TODO: Parse response and match to Product list

            return new List<Product>(); // Return matched products
        }

        private string GeneratePrompt(string requirement, List<Product> products)
        {
            var sb = new StringBuilder();
            sb.AppendLine("A user has the following requirement:");
            sb.AppendLine(requirement);
            sb.AppendLine("Here is the list of available products with their features:");

            foreach (var product in products)
            {
                sb.AppendLine($"- Product: {product.Name}, Price: {product.Price}, Vendor: {product.VendorName}");
                foreach (var feature in product.ProductFeatures.OrderBy(f => f.OrderBy))
                {
                    sb.AppendLine($"   - {feature.Name}: {feature.Value}");
                }
            }

            sb.AppendLine("Based on the user's requirement, return the most suitable products (names only).");

            return sb.ToString();
        }
    }
}