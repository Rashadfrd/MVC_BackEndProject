using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.DAL;
using Riode.ViewModels;

namespace Riode.Controllers
{
    public class ProductController : Controller
    {
        RiodeContext _context { get; }
        public ProductController(RiodeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest();
            ProductVM vm = new ProductVM();
            vm.Product = _context.Products
                .Include(x => x.ProductImages)
                .Include(x =>x.Category)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.ProductBadges)
                .ThenInclude(x => x.Badge).FirstOrDefault(x=>x.Id == id);
            if (vm.Product is null) return NotFound();
            var filter = vm.Product.Category.Name;
            vm.Products = _context.Products.Include(x => x.ProductImages).Include(x=>x.Category).Where(x => x.Category.Name == filter && x.Id != id);
            return View(vm);
        }
    }
}
