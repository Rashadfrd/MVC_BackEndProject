using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.DAL;
using Riode.ViewModels;
using System.Diagnostics;

namespace Riode.Controllers
{
    public class HomeController : Controller
    {
        RiodeContext _context { get; }
        public HomeController(RiodeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.DiscountBadge = _context.Badges.FirstOrDefault(x => x.Id == 4);
            HomeVM vm = new HomeVM();
            vm.Brands = _context.Brands.Where(x => x.IsDeleted == false);
            vm.Features = _context.Features.Where(x => x.IsDeleted == false);
            vm.Sliders = _context.Sliders.Where(x=>x.IsDeleted == false).OrderBy(x=>x.Order);
            vm.Categories = _context.Categories.Include(x => x.Products).Where(x => x.IsDeleted == false && x.IsMain == true);
            vm.Products = _context.Products
                .Include(x => x.Category)
                .Include(x=>x.Brand)
                .Include(x => x.ProductImages)
                .Include(x=>x.ProductBadges)
                .ThenInclude(x=>x.Badge)
                .Where(x=>x.IsDeleted == false);
            vm.Advertisements = _context.Advertisements.Where(x => x.IsDeleted == false);
            return View(vm);
        }
        public IActionResult Modal(int? id)
        {
            if (id is null) return BadRequest();
            var product = _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.Category)
                .Include(x => x.ProductBadges)
                .Include(x => x.Brand)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .FirstOrDefault(x => x.Id == id);
            if (product is null) return NotFound();
            return PartialView("_ModalPartialView", product);
        }


        public IActionResult AboutUs()
        {
            var aboutUs = _context.AboutUs;
            ViewBag.BrandCount = _context.Brands.Count();
            ViewBag.AdminsCount = _context.AppUsers.Where(x => x.IsAdmin == true).Count();        
            return View(aboutUs);
        }
    }
}