using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.DAL;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        RiodeContext _context { get; }

        public ProductController(RiodeContext context)
        {
            _context = context;
        }

        // GET: ProductController
        public ActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var products = _context.Products
                 .Include(x => x.ProductImages)
                 .Include(x => x.Category)
                 .Include(x=>x.Brand)
                 .Include(x => x.ProductColors)
                 .ThenInclude(x => x.Color)
                 .Include(x => x.ProductBadges)
                 .ThenInclude(x => x.Badge);
            return View(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
