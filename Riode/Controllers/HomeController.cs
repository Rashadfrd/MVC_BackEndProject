using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Riode.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}