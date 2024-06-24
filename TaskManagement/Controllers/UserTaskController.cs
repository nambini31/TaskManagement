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


        [HttpPost]
        public async Task<IActionResult> UserTaskList(FiltreUserTask filter)
        {
            try
            {
                var data = await _SUserTask.GetUserTaskVM(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteUserTask(int userTaskId)
        {
            try
            {
                await _SUserTask.DeleteUserTaskById(userTaskId);

                var responseData = new { message = "Success" };

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        [HttpPost]
        public async Task<IActionResult> ModalUserTaskEdit(int userTaskId)
        {
            try
            {
                UserTask userTask = await _SUserTask.GetUserTaskById(userTaskId);
                return PartialView("_modalEdit" ,userTask);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateUserTask(UserTask userTask)
        {
            try
            {
                userTask.userId = 1;
                await _SUserTask.UpdateUserTask(userTask);

                var responseData = new { message = "Success" };

                return Ok(responseData);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> GenerateExcelUserTask(FiltreUserTask filter)
        {
            try
            {

                await _SUserTask.GenerateUserTask(filter);

                var responseData = new { message = "Success" };

                return Ok(responseData);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
