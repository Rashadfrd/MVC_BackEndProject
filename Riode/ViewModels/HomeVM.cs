using Riode.Models;

namespace Riode.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Feature> Features { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}
