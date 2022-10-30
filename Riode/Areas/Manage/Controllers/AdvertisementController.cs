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
    public class AdvertisementController : Controller
    {
        RiodeContext _context { get; }
        IWebHostEnvironment _env { get; }

        public AdvertisementController(RiodeContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: AdvertisementController
        public ActionResult Index()
        {
            var advert = _context.Advertisements;
            return View(advert);
        }

        // GET: AdvertisementController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdvertisementController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Advertisement advert)
        {
            if (advert.ImageFile is null) ModelState.AddModelError("ImageFile", "Please, choose image");
            if (!ModelState.IsValid) return View();
            var file = advert.ImageFile;
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
            string fileName = file.SaveImage(_env.WebRootPath, "images/demos/demo1/banners/");
            advert.ImageUrl = fileName;
            advert.IsDeleted = false;
            _context.Advertisements.Add(advert);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: AdvertisementController/Edit/5
        public ActionResult Update(int id)
        {
            var advert = _context.Advertisements.Find(id);
            return View(advert);
        }

        // POST: AdvertisementController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int? id, Advertisement advert)
        {
            if (id != advert.Id || id is null) return BadRequest();
            if (advert.ImageFile is null) ModelState.AddModelError("ImageFile", "Please, choose image");
            if (!ModelState.IsValid) return View(advert);
            var editadvert = _context.Advertisements.Find(id);
            if (editadvert is null) return BadRequest();
            var file = advert.ImageFile;
            if (!file.CheckFileExtension("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be image");
                return View(advert);
            }
            if (file.CheckFileSize(2))
            {
                ModelState.AddModelError("ImageFile", "File size can not be more than 2 mb!");
                return View(advert);
            }
            string fileName = file.SaveImage(_env.WebRootPath, "images/demos/demo1/banners/");
            string path = Path.Combine(_env.WebRootPath, "images/demos/demo1/banners/", editadvert.ImageUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            advert.ImageUrl = fileName;
            editadvert.ImageUrl = advert.ImageUrl;
            editadvert.Name = advert.Name;
            editadvert.Title = advert.Title;
            editadvert.Place = advert.Place;
            editadvert.IsModified = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: AdvertisementController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var advert = _context.Advertisements.Find(id);
            if (advert is null) return NotFound();
            if (advert.IsDeleted == false)
            {
                advert.IsDeleted = true;
            }
            else
            {
                string path = Path.Combine(_env.WebRootPath, "images/demos/demo1/banners/", advert.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.Advertisements.Remove(advert);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
