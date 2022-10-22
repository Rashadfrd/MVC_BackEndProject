using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.Models;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
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
        public ActionResult Index()
        {
            var features = _context.Features;
            return View(features);
        }

        // GET: FeaturesController/Create
        public ActionResult Create()
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FeaturesController/Edit/5
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
