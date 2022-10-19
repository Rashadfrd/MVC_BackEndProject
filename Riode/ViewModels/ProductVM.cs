using Riode.Models;

namespace Riode.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
