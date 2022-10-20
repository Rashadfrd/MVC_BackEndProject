using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.Models;
using Riode.Utilities.Extensions;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
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
        public IActionResult Create(Slider? slider)
        {
            if (slider is null) return BadRequest();
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
            string path = Path.Combine(_env.WebRootPath, "images/demos/demo1/slides/", slider.ImageUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            if (slider.IsDeleted == false)
            {
                slider.IsDeleted = true;
            }
            else
            {
            _context.Sliders.Remove(slider);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
