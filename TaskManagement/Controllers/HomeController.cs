using Application.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ArticleServiceRepository _articleServiceRepository;

        public HomeController(ILogger<HomeController> logger , ArticleServiceRepository articleServiceRepository)
        {
            _logger = logger;
            _articleServiceRepository = articleServiceRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
