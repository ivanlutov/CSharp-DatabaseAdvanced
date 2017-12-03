using System.Collections.Generic;
using ProductShop.Models;

namespace ProductShop.Client.ModelsDtos
{
    public class CategoryDto
    {
        public string Category { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}