using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riode.DAL;

namespace Riode.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BadgeController : Controller
    {
        RiodeContext _context { get; }

        public BadgeController(RiodeContext context)
        {
            _context = context;
        }
        // GET: BadgeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BadgeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BadgeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BadgeController/Create
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

        // GET: BadgeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BadgeController/Edit/5
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

        // GET: BadgeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BadgeController/Delete/5
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
