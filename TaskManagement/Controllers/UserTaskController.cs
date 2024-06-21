using Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class UserTaskController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SUserTask _SUserTask;

        public UserTaskController(ILogger<HomeController> logger, SUserTask _SUserTask)
        {
            _logger = logger;

            this._SUserTask = _SUserTask;


        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserTaskList()
        {
            var data = await _SUserTask.GetUserTaskVM();

            return Json(data);
        }
    }
}
