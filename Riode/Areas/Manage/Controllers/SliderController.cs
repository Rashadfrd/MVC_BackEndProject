using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.Models;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly RiodeContext _context;
        public SliderController(RiodeContext context)
        {
            _context = context;
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
            if(!ModelState.IsValid) return View();
            if (slider is null) return BadRequest();
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
            _context.Sliders.Remove(slider);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
