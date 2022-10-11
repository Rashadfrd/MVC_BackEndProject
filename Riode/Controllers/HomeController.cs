using Microsoft.AspNetCore.Mvc;
using Riode.DAL;
using Riode.ViewModels;
using System.Diagnostics;

namespace Riode.Controllers
{
    public class HomeController : Controller
    {
        RiodeContext _context { get; }
        public HomeController(RiodeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM vm = new HomeVM();
            vm.Features = _context.Features;
            vm.Sliders = _context.Sliders.Where(x=>x.IsDeleted == false).OrderBy(x=>x.Order);
            return View(vm);
        }

    }
}