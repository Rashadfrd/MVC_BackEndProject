using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.Models;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class FeaturesController : Controller
    {

        private readonly RiodeContext _context;
        private readonly IWebHostEnvironment _env;
        public FeaturesController(RiodeContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: FeaturesController
        public IActionResult Index()
        {
            var features = _context.Features;
            return View(features);
        }

        // GET: FeaturesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        public IActionResult Create(Feature feature)
        {
            if (!ModelState.IsValid) return View();
            feature.IsDeleted = false;
            _context.Features.Add(feature);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: FeaturesController/Edit/5
        public IActionResult Update(int id)
        {
            var features = _context.Features.Find(id);
            return View(features);
        }

        // POST: FeaturesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int? id, Feature feature)
        {
            if (id != feature.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(feature);
            var editfeature = _context.Features.Find(id);
            if (editfeature is null) return BadRequest();
            editfeature.Title = feature.Title;
            editfeature.Description = feature.Description;
            editfeature.Icon = feature.Icon;
            editfeature.IsModified = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: FeaturesController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var feature = _context.Features.Find(id);
            if (feature is null) return NotFound();
            if (feature.IsDeleted == false)
            {
                feature.IsDeleted = true;
            }
            else
            {
                _context.Features.Remove(feature);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
