using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.Models;
using Riode.Utilities.Extensions;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        RiodeContext _context { get; }
        private readonly IWebHostEnvironment _env;

        public CategoryController(RiodeContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
            var categories = _context.Categories;
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (category.MainCategoryId is null)
            {
                ModelState.AddModelError("MainCategoryId", "MainCategoryId field is required");
                return View();
            }
            if (!ModelState.IsValid) return View();
            if (category.ImageFile != null)
            {
                var file = category.ImageFile;
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
                string fileName = file.SaveImage(_env.WebRootPath, "images/categories/");
                category.ImageUrl = fileName;
            }
            if (category.IsMain == true)
            {
                category.MainCategoryId = null;
            }
            category.IsDeleted = false;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        public ActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int? id, Category category)
        {
            if (id != category.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(category);
            var editcategory = _context.Categories.Find(id);
            if (editcategory is null) return BadRequest();
            if (category.ImageFile != null)
            {
            var file = category.ImageFile;
            if (!file.CheckFileExtension("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be image");
                return View(category);
            }
            if (file.CheckFileSize(2))
            {
                ModelState.AddModelError("ImageFile", "File size can not be more than 2 mb!");
                return View(category);
            }
            string fileName = file.SaveImage(_env.WebRootPath, "images/categories/");
            category.ImageUrl = fileName;
            }
            if (editcategory.ImageUrl != null)
            {
            string path = Path.Combine(_env.WebRootPath, "images/categories/", editcategory.ImageUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            }
            if (category.IsMain == true)
            {
                category.MainCategoryId = null;
            }
            editcategory.MainCategoryId = category.MainCategoryId;
            editcategory.IsMain = category.IsMain;
            editcategory.ImageUrl = category.ImageUrl;
            editcategory.Name = category.Name;
            editcategory.IsModified = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var category = _context.Categories.Find(id);
            if (category is null) return NotFound();
            if (category.IsDeleted == false)
            {
                category.IsDeleted = true;
            }
            else
            {
                if (category.ImageFile != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/categories/", category.ImageUrl);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                _context.Categories.Remove(category);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
