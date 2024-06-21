using Application.Service;
using Application.Services;
using Domain.DTO.ViewModels;
using Domain.Entity;
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
        public  IActionResult Index()
        {

            return View();
        } 
        public  string getListUser()
        {
            IEnumerable<User> data = _UserService.GetUser();
            var option = "";
            foreach (var item in data)
            {
                 option += "<option value= '"+ item.UserId+"' > "+ item.Name +" </option>";
            }
            return option;
        }

        [HttpPost]
        public async Task<IActionResult> UserTaskList()
        {
            var data = await _SUserTask.GetUserTaskVM();

            return Json(data);
        }
    }
}
