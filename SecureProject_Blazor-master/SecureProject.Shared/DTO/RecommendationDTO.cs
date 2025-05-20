using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureProject.Shared.DTO
{
    public class RecommendationDTO
    {
        public string RecommendedReply { get; set; }
        public List<Product> RecommendedProducts { get; set; }
    }
}
