using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureProject.Shared.DTO
{
    public class ProductFeatureAddEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int OrderBy { get; set; }

    }
}
