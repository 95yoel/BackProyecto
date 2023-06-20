using AsturTravel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AsturTravel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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