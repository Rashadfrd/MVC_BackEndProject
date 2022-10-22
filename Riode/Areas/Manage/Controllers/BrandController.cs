using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.Models;
using Riode.Utilities.Extensions;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrandController : Controller
    {
        private readonly RiodeContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandController(RiodeContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: BrandController
        public ActionResult Index()
        {
            var brand = _context.Brands;
            return View(brand);
        }

        // GET: BrandController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (brand.ImageFile is null) ModelState.AddModelError("ImageFile", "Please, choose image");
            if (!ModelState.IsValid) return View();
            var file = brand.ImageFile;
            if (!file.CheckFileExtension("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be image");
                return View();
            }
            if (file.CheckFileSize(2))
            {
                ModelState.AddModelError("ImageFile", "File size can not be more than 2 mb!");
                return View();
            }
            string fileName = file.SaveImage(_env.WebRootPath, "images/brands/");
            brand.ImageUrl = fileName;
            brand.IsDeleted = false;
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: BrandController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BrandController/Edit/5
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

        // GET: BrandController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var brand = _context.Brands.Find(id);
            if (brand is null) return NotFound();
            if (brand.IsDeleted == false)
            {
                brand.IsDeleted = true;
            }
            else
            {
                string path = Path.Combine(_env.WebRootPath, "images/brands/", brand.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.Brands.Remove(brand);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
