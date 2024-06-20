
using Application.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SUserTask _SUserTask;

        public HomeController(ILogger<HomeController> logger , SUserTask _SUserTask)
        {
            _logger = logger;

            this._SUserTask = _SUserTask ;


        }

        public async Task<IActionResult> Index()
        {
            var data = await _SUserTask.GetUserTaskVM();

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
