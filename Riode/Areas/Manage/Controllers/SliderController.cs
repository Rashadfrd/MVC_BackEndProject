using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.Models;
using Riode.Utilities.Extensions;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SliderController : Controller
    {
        private readonly RiodeContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(RiodeContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var slider = _context.Sliders;
            return View(slider);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.ImageFile is null) ModelState.AddModelError("ImageFile", "Please, choose image");
            if (!ModelState.IsValid) return View();
            var file = slider.ImageFile;
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
            string fileName = file.SaveImage(_env.WebRootPath, "images/demos/demo1/slides/");
            slider.ImageUrl = fileName;
            slider.IsDeleted = false;
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var slider = _context.Sliders.Find(id);
            if (slider is null) return NotFound();
            if (slider.IsDeleted == false)
            {
                slider.IsDeleted = true;
            }
            else
            {
                string path = Path.Combine(_env.WebRootPath, "images/demos/demo1/slides/", slider.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.Sliders.Remove(slider);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            var slider = _context.Sliders.Find(id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        public IActionResult Update(int? id, Slider slider)
        {
            if (id != slider.Id || id is null) return BadRequest();
            if (slider.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Please, choose image");
                return View();
            }
            if(!ModelState.IsValid) return View(slider);
            var editSlider = _context.Sliders.Find(id);
            if (editSlider is null) return BadRequest();
            var file = slider.ImageFile;
            if (!file.CheckFileExtension("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be image");
                return View(slider);
            }
            if (file.CheckFileSize(2))
            {
                ModelState.AddModelError("ImageFile", "File size can not be more than 2 mb!");
                return View(slider);
            }
            string fileName = file.SaveImage(_env.WebRootPath, "images/demos/demo1/slides/");
            string path = Path.Combine(_env.WebRootPath, "images/demos/demo1/slides/", editSlider.ImageUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            slider.ImageUrl = fileName;
            editSlider.ImageUrl = slider.ImageUrl;
            editSlider.Order = slider.Order;
            editSlider.Description = slider.Description;
            editSlider.Title = slider.Title;
            editSlider.SubTitle = slider.SubTitle;
            editSlider.Place = slider.Place;
            editSlider.IsModified = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        } 

    }
}
