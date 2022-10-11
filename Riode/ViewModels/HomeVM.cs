using Riode.Models;

namespace Riode.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Feature> Features { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }
    }
}
