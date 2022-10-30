using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.DAL;
using Riode.Models;
using Riode.Utilities.Extensions;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {
        RiodeContext _context { get; }
        private readonly IWebHostEnvironment _env;

        public ProductController(RiodeContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: ProductController
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var products = _context.Products
                 .Include(x => x.ProductImages)
                 .Include(x => x.Category)
                 .Include(x => x.Brand)
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
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Where(x => x.IsMain == false);
            ViewBag.MainCategories = _context.Categories.Where(x => x.IsMain == true);
            ViewBag.Colors = _context.Colors;
            ViewBag.Brands = _context.Brands;
            ViewBag.Badges = _context.Badges;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product prod)
        {
            ViewBag.Categories = _context.Categories.Where(x => x.IsMain == false);
            ViewBag.MainCategories = _context.Categories.Where(x => x.IsMain == true);
            ViewBag.Colors = _context.Colors;
            ViewBag.Brands = _context.Brands;
            ViewBag.Badges = _context.Badges;
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (prod.MainImage is null)
            {
                ModelState.AddModelError("MainImage", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            if (prod.DiscountPercent < 0 && prod.DiscountPercent > 100)
            {
                ModelState.AddModelError("DiscountPercent", "Discount percent must be between 0 and 100");
                return View();
            }

            #region Images
            var productImgs = prod.OtherImages;
            List<ProductImage> images = new List<ProductImage>();
            if (productImgs != null)
            {
                foreach (var img in productImgs)
                {
                    if (!img.CheckFileExtension("image/"))
                    {
                        ModelState.AddModelError("OtherImages", "File must be image");
                        return View();
                    }
                    if (img.CheckFileSize(2))
                    {
                        ModelState.AddModelError("OtherImages", "File size can not be more than 2 mb!");
                        return View();
                    }
                }
                foreach (var img in productImgs)
                {
                    string fileNames = img.SaveImage(_env.WebRootPath, "images/products/");
                    images.Add(new()
                    {
                        ImageUrl = fileNames,
                        StatusImage = null,
                        Product = prod
                    }); ;
                }
            }
            #endregion

            #region Main Image
            var mainImg = prod.MainImage;

            if (!mainImg.CheckFileExtension("image/"))
            {
                ModelState.AddModelError("MainImage", "File must be image");
                return View();
            }
            if (mainImg.CheckFileSize(2))
            {
                ModelState.AddModelError("MainImage", "File size can not be more than 2 mb!");
                return View();
            }
            string fileName = mainImg.SaveImage(_env.WebRootPath, "images/products/");
            images.Add(new()
            {
                ImageUrl = fileName,
                StatusImage = true,
                Product = prod
            });
            #endregion

            if (prod.ColorsIds != null)
            {
                prod.ProductColors = new List<ProductColor>();
                foreach (var item in prod.ColorsIds)
                {
                    prod.ProductColors.Add(new() { Product = prod, ColorId = item });
                }
            }
            if (prod.BadgesIds != null)
            {
                prod.ProductBadges = new List<ProductBadge>();
                foreach (var item in prod.BadgesIds)
                {
                    prod.ProductBadges.Add(new() { Product = prod, BadgeId = item });
                }
            }

            //prod.SKU = prod.Name.Substring(0, 2) + prod.CategoryId.ToString();
            Random rd = new Random();

            int rand_num = rd.Next(0, 2000);
            prod.SKU = rand_num + "SK123";
            prod.ProductImages = images;
            prod.IsDeleted = false;
            prod.CreatedDate = DateTime.Now;
            _context.Products.Add(prod);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Edit/5
        public IActionResult Update(int? id)
        {
            ViewBag.Categories = _context.Categories.Where(x => x.IsMain == false);
            ViewBag.MainCategories = _context.Categories.Where(x => x.IsMain == true);
            ViewBag.Colors = _context.Colors;
            ViewBag.Brands = _context.Brands;
            ViewBag.Badges = _context.Badges;
            if (id is null) return BadRequest();
            var prd = _context.Products.Include(x => x.ProductImages)
                .Include(x => x.Category)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.ProductBadges)
                .ThenInclude(x => x.Badge).FirstOrDefault(x => x.Id == id);
            if (prd is null) return NotFound();
            return View(prd);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        public IActionResult Update(int? id, Product prd)
        {
            ViewBag.Categories = _context.Categories.Where(x => x.IsMain == false);
            ViewBag.MainCategories = _context.Categories.Where(x => x.IsMain == true);
            ViewBag.Colors = _context.Colors;
            ViewBag.Brands = _context.Brands;
            ViewBag.Badges = _context.Badges;
            if (id != prd.Id || id is null) return BadRequest();
            if (prd.MainImage is null)
            {
                ModelState.AddModelError("MainImage", "Please, choose image");
                return View(prd);
            }
            if (prd.OtherImages is null)
            {
                ModelState.AddModelError("OtherImages", "Please, choose image");
                return View(prd);
            }
            if (prd.DiscountPercent < 0 && prd.DiscountPercent > 100)
            {
                ModelState.AddModelError("DiscountPercent", "Discount percent must be between 0 and 100");
                return View(prd);
            }
            if (!ModelState.IsValid) return View(prd);
            var product = _context.Products.Find(id);
            if (product is null) return BadRequest();


            var productColors = _context.ProductColors.Where(c => c.ProductId == product.Id).ToList();
            if (productColors != null)
            {
                foreach (var item in productColors)
                {
                    _context.ProductColors.Remove(item);
                    _context.SaveChanges();
                }
            }

            var productBadges = _context.ProductBadges.Where(x => x.ProductId == product.Id).ToList();
            if (productBadges != null)
            {
                foreach (var item in productBadges)
                {
                    _context.ProductBadges.Remove(item);
                    _context.SaveChanges();
                }
            }

            var photos = _context.ProductImages
                .Where(p => p.ProductId == product.Id)
                .ToList();

            foreach (var item in photos)
            {
                string path = Path.Combine(_env.WebRootPath, "images/products/", item.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.ProductImages.Remove(item);
                _context.SaveChanges();
            }
            var productImgs = prd.OtherImages;
            List<ProductImage> images = new List<ProductImage>();
            if (productImgs != null)
            {

                foreach (var img in productImgs)
                {
                    if (!img.CheckFileExtension("image/"))
                    {
                        ModelState.AddModelError("ImageFile", "File must be image");
                        return View();
                    }
                    if (img.CheckFileSize(2))
                    {
                        ModelState.AddModelError("ImageFile", "File size can not be more than 2 mb!");
                        return View();
                    }
                }
                foreach (var img in productImgs)
                {
                    string fileNames = img.SaveImage(_env.WebRootPath, "images/products/");
                    images.Add(new()
                    {
                        ImageUrl = fileNames,
                        StatusImage = null,
                        Product = prd
                    }); ;
                }
            }

            var mainImg = prd.MainImage;

            if (!mainImg.CheckFileExtension("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be image");
                return View();
            }
            if (mainImg.CheckFileSize(2))
            {
                ModelState.AddModelError("ImageFile", "File size can not be more than 2 mb!");
                return View();
            }
            string fileName = mainImg.SaveImage(_env.WebRootPath, "images/products/");
            images.Add(new()
            {
                ImageUrl = fileName,
                StatusImage = true,
                Product = prd
            });

            if (prd.ColorsIds != null)
            {
                prd.ProductColors = new List<ProductColor>();
                foreach (var item in prd.ColorsIds)
                {
                    prd.ProductColors.Add(new() { Product = prd, ColorId = item });
                }
            }
            if (prd.BadgesIds != null)
            {
                prd.ProductBadges = new List<ProductBadge>();
                foreach (var item in prd.BadgesIds)
                {
                    prd.ProductBadges.Add(new() { Product = prd, BadgeId = item });
                }
            }
            product.BrandId = prd.BrandId;
            product.CategoryId = prd.CategoryId;
            product.MainCategoryId = prd.MainCategoryId;
            product.ProductColors = prd.ProductColors;
            product.ProductBadges = prd.ProductBadges;
            product.ProductImages = images;
            product.IsModified = DateTime.UtcNow;
            product.Description = prd.Description;
            product.Name = prd.Name;
            product.InitialPrice = prd.InitialPrice;
            product.DiscountPercent = prd.DiscountPercent;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var prod = _context.Products.Find(id);
            if (prod is null) return NotFound();
            if (prod.IsDeleted == false)
            {
                prod.IsDeleted = true;
            }
            else
            {
                var productColors = _context.ProductColors.Where(c => c.ProductId == id).ToList();
                if (productColors != null)
                {
                    foreach (var item in productColors)
                    {
                        _context.ProductColors.Remove(item);
                        _context.SaveChanges();
                    }
                }

                var productBadges = _context.ProductBadges.Where(x => x.ProductId == id).ToList();
                if (productBadges != null)
                {
                    foreach (var item in productBadges)
                    {
                        _context.ProductBadges.Remove(item);
                        _context.SaveChanges();
                    }
                }
                if (prod.ProductImages != null)
                {
                    var photos = _context.ProductImages
                        .Where(p => p.ProductId == prod.Id)
                        .ToList();

                    foreach (var item in photos)
                    {
                        string path = Path.Combine(_env.WebRootPath, "images/products/", item.ImageUrl);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        _context.ProductImages.Remove(item);
                        _context.SaveChanges();
                    }
                }

                _context.Products.Remove(prod);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
