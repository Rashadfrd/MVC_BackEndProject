using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            ViewBag.maxId = _context.Products.Max(x => x.Id);
            return View(vm);
        }

        public IActionResult Basket(int? id)
        {
            if (id is null) return BadRequest();
            if (!_context.Products.Any(x => x.Id == id)) return NotFound();
            List<BasketItem> basket;
            if (HttpContext.Request.Cookies["Basket"] is null)
            {
                basket = new List<BasketItem>();
            }
            else
            {
               basket =  JsonConvert.DeserializeObject<List<BasketItem>>(HttpContext.Request.Cookies["Basket"]);
            }
            var existingProduct = basket.Find(x=>x.Id == id);
            if (existingProduct is null)
            {
                basket.Add(new BasketItem { Count = 1, Id = (int)id });
            }
            else
            {
                existingProduct.Count++;
            }
            HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basket), new CookieOptions { MaxAge = TimeSpan.MaxValue });
            return RedirectToAction(nameof(Details), new {id = id });
        }
    }
}
