
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [Authorize]
    public class Home__Controller : Controller
    {
        private readonly ILogger<Home__Controller> _logger;

        private readonly SUserTaskRepository _SUserTask;

        public Home__Controller(ILogger<Home__Controller> logger , SUserTaskRepository _SUserTask)
        {
            _logger = logger;

            this._SUserTask = _SUserTask ;


        }

        public async Task<IActionResult> Index()
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
