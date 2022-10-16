using Riode.DAL;
using Riode.ViewModels;
using Newtonsoft.Json;
using Riode.Models;
using Microsoft.EntityFrameworkCore;

namespace Riode.Service
{
    public class LayoutService
    {
        RiodeContext _context { get; }
        IHttpContextAccessor _accessor { get; }
        public LayoutService(RiodeContext context,IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }


        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(p => p.Key, p => p.Value);
        }

        public BasketVM Basket()
        {
            var basket = new BasketVM { Products = new(), TotalPrice = 0 };
            List<BasketItem> basketItems;
            if (_accessor.HttpContext.Request.Cookies["Basket"] is null)
            {
                basketItems = new List<BasketItem>();
            }
            else
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(_accessor.HttpContext.Request.Cookies["Basket"]);
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
            return basket;

        }

    }
}
