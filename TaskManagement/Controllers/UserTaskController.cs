using Application.Service;
using Application.Services;
using Domain.DTO;
using Domain.DTO.ViewModels;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagement.Controllers
{
    public class UserTaskController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SUserTaskRepository _SUserTask;
        private readonly UserServiceRepository _UserService;

        public UserTaskController(ILogger<HomeController> logger, SUserTaskRepository _SUserTask , UserServiceRepository _UserService)
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
            IEnumerable<UserListWithRole> data = _UserService.GetUser();
            var option = "";
            foreach (var item in data)
            {
                 option += "<option value= '"+ item.UserId+"' > "+ item.Name +" </option>";
            }
            return option;
        }

        [HttpPost]
        public async Task<IActionResult> UserTaskList(FiltreUserTask filter)
        {
            try
            {
                var data = await _SUserTask.GetUserTaskVM(filter);

                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
