using GenerativeAI;
using GenerativeAI.Core;
using SecureProject.Shared;
using SecureProject.Shared.DTO;
using System.Text.RegularExpressions;

namespace SecureProject.Client.Service
{
    public class GeminiProductRecommendationService
    {
        private readonly GenerativeModel _model;
        private readonly List<Product> _products;

        public GeminiProductRecommendationService(string apiKey, List<Product> products)
        {
            IPlatformAdapter platformAdapter = new GoogleAIPlatformAdapter(apiKey);
            _model = new GenerativeModel(
                platform: platformAdapter,
                model: "gemini-2.0-flash"
            );
            _products = products;
        }


        public async Task<RecommendationDTO> GetRecommendations(string userRequirement)
        {
            string productDetails = string.Join("\n", _products.Select(p =>
                $"ID: {p.Id}, Name: {p.Name}, Description: {p.Description}, VendorName: {p.VendorName}, Price: {p.Price}, Features: {string.Join(", ", p.ProductFeatures.Select(f => $"{f.Name}: {f.Value}"))}"));

            string prompt = $"Given the following user requirement: '{userRequirement}'. " +
                            $"And the following list of products:\n{productDetails}\n\n" +
                            "Create a proper security posture according to the user requirments , which includes one firewall,one Antivirus/Anti-malware software, one Privilege Access Management (PAM), one Data Loss Prevention (DLP), one Web Application Firewall (WAF), one SIEM (Security Information and Event Management) \n\n Please list the IDs of the most suitable products names and briefly explain why?\n\n" +
                            "ID should replace with product Name (without product ID).\n\n Return selected product ID as array (eg: {1,3,4}).";

            var response = await _model.GenerateContentAsync(prompt);
            var responseText = response.Candidates.FirstOrDefault()?.Content?.Parts.FirstOrDefault()?.Text;

            if (!string.IsNullOrEmpty(responseText))
            {
                var recommendedIds = ParseRecommendedProductIds(responseText);
                List<Product> recomondedProducts = new();

                if (recommendedIds != null && recommendedIds.Count() > 0)
                {
                    foreach (var id in recommendedIds)
                    {
                        var product = _products.FirstOrDefault(p => p.Id == id);
                        if (product != null)
                        {
                            recomondedProducts.Add(product);
                            // Replace "ID: X, Name: Y..." with just "Product: Y"
                            string idPattern = $@"\* \*\*ID: {id}, Name: {Regex.Escape(product.Name)}[^*]*\*\*";
                            string replacement = $"* **Product: {product.Name}**";
                            responseText = Regex.Replace(responseText, idPattern, replacement);
                            responseText = responseText.Replace($@"* **ID: {id}**", "");
                            responseText = responseText.Replace($@"**ID: {id}**", "");
                            responseText = responseText.Replace($@":**", "- ");
                        }
                    }


                }
                try
                {
                    responseText = responseText.Split("{")[0].Replace("`", "").Replace("Selected Product IDs-", " ");
                }
                catch { }

                // Regex pattern to match **<product name>*
                string pattern = @"\*\*(.*?)\*";

                var matches = Regex.Matches(responseText, pattern);

                foreach (Match match in matches)
                {
                    string product = match.Groups[1].Value.Trim();

                    if (!recomondedProducts.Any(c => c.Name.Contains(product)))
                    {
                        var productObj = _products.FirstOrDefault(p => p.Name.Contains(product));
                        if (productObj != null)
                        {
                            recomondedProducts.Add(productObj);
                        }
                    }
                }

                RecommendationDTO returnObj = new RecommendationDTO()
                {
                    RecommendedReply = responseText.Replace("Selected Product IDs-", " ").Replace("** * **", " ").Replace("*", " "),
                    RecommendedProducts = recomondedProducts
                };
                return returnObj;
            }

            return null;
        }

        private List<int> ParseRecommendedProductIds(string text)
        {
            var ids = new List<int>();
            var parts = text.ToLower().Split(new[] { "{", "}", "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (int.TryParse(part.Trim(), out int id))
                {
                    ids.Add(id);
                }
            }
            return ids;
        }
    }
}