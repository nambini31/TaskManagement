
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SUserTaskRepository _SUserTask;

        public HomeController(ILogger<HomeController> logger , SUserTaskRepository _SUserTask)
        {
            _logger = logger;

            this._SUserTask = _SUserTask ;
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
