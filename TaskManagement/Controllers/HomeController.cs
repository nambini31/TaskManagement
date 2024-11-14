
using Application.Interface;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SUserTaskRepository _SUserTask;
        private readonly ITasksService _tasksService;
        private readonly IProjectService _projectService;
        public HomeController(ILogger<HomeController> logger , SUserTaskRepository _SUserTask, ITasksService tasksService, IProjectService projectService)
        {
            _logger = logger;

            this._SUserTask = _SUserTask ;
            _tasksService = tasksService;
            _projectService = projectService;
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            @ViewData["titrePage"] = "Dashboard";
            return View("Dashboard");
        }

        [HttpPost]
        public async Task<JsonResult> ChartProjectProcess()
        {
            var tasks = await _tasksService.ChartProjectProcess();
            return Json(tasks);
        }

        [HttpPost]
        public async Task<JsonResult> ChartTaskProcessByProject()
        {
            var tasks = await _tasksService.ChartTaskProcessByProject();
            return Json(tasks);
        }


        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
