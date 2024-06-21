using Application.Service;
using Application.Services;
using Domain.DTO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagement.Controllers
{
    public class UserTaskController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SUserTask _SUserTask;
        private readonly UserServiceRepository _UserService;

        public UserTaskController(ILogger<HomeController> logger, SUserTask _SUserTask , UserServiceRepository _UserService)
        {
            _logger = logger;

            this._SUserTask = _SUserTask;
             this._UserService = _UserService;

        }
        public IActionResult Index()
        {

            IEnumerable<SelectListItem> selectCategory = _UserService. .Select(u => new SelectListItem
            {
                Text= u.design,
                Value = u.id.ToString()
            }

            );

            UserTaskVM vm = new UserTaskVM()
            {
                selectUser = 
            };


            return View();
        }
        public async Task<IActionResult> UserTaskList()
        {
            var data = await _SUserTask.GetUserTaskVM();

            return Json(data);
        }
    }
}
