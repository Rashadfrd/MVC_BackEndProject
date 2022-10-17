using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Riode.DAL;
using Riode.Models;
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
                .Include(x => x.Category)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.ProductBadges)
                .ThenInclude(x => x.Badge).FirstOrDefault(x => x.Id == id);
            if (vm.Product is null) return NotFound();
            var filter = vm.Product.Category.Name;
            vm.Products = _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Where(x => x.Category.Name == filter && x.Id != id);
            ViewBag.maxId = _context.Products.Max(x => x.Id);
            return View(vm);
        }


        public IActionResult DeleteBasket(int? id)
        {
            if (id is null) return BadRequest();
            List<BasketItem> basketItems;
            if (HttpContext.Request.Cookies["Basket"] is null)
            {
                basketItems = new List<BasketItem>();
            }
            else
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(HttpContext.Request.Cookies["Basket"]);
            }
            var isItemExist = basketItems.Find(x => x.Id == id);
            if (isItemExist is null)
            {
                return NotFound();
            }
            else
            {
                basketItems.Remove(isItemExist);
            }
            HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basketItems), new CookieOptions { MaxAge = TimeSpan.MaxValue });
            return RedirectToAction(nameof(GetBasket));

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
                basket = JsonConvert.DeserializeObject<List<BasketItem>>(HttpContext.Request.Cookies["Basket"]);
            }
            var existingProduct = basket.Find(x => x.Id == id);
            if (existingProduct is null)
            {
                basket.Add(new BasketItem { Count = 1, Id = (int)id });
            }
            else
            {
                existingProduct.Count++;
            }
            HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basket), new CookieOptions { MaxAge = TimeSpan.MaxValue });
            return RedirectToAction(nameof(GetBasket));
        }

        public IActionResult GetBasket()
        {
            var basket = new BasketVM { Products = new(), TotalPrice = 0 };
            List<BasketItem> basketItems;
            if (HttpContext.Request.Cookies["Basket"] is null)
            {
                basketItems = new List<BasketItem>();
            }
            else
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(HttpContext.Request.Cookies["Basket"]);
            }
            foreach (var item in basketItems)
            {
                Product p = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                if (p != null)
                {
                    basket.Products.Add(new ProductBasketItemVM
                    {
                        Product = p,
                        Count = item.Count
                    });
                    basket.TotalPrice += p.InitialPrice * (100 - p.DiscountPercent) / 100 * item.Count;
                }
            }
            return PartialView("BasketPartialView", basket);

        }
    }
}
