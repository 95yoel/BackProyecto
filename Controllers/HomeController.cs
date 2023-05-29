using AsturTravel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AsturTravel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //return View("~/Views/Inicio/Inicio.cshtml");
            return View();
        }

        public IActionResult Privacy()
        {
            return Redirect("http://www.google.com");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DocumentacionHttp()
        {
            return PartialView("PartialsHomeAdmin/_PartialDocsHttp");
        }




    }
}