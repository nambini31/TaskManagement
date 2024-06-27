using Application.Services;
using Domain.DTO;
using Domain.DTO.ViewModels;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace TaskManagement.Controllers
{
   [Authorize]
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
                if (User.FindFirstValue(ClaimTypes.Role).ToString() != "Admin")
                {
                    var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    filter.userId = new List<int> { currentUserId };
                }
                var data = await _SUserTask.GetUserTaskVM(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public IActionResult Create()
        {
            return View();

        }

        public async Task<IActionResult> Create(UserTask model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _SUserTask.AddUserTask(model);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, error = "Invalid model state" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserTask(int userTaskId)
        {
            try
            {
                var userConnected = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _SUserTask.DeleteUserTaskById(userTaskId, userConnected);

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
        public async Task<IActionResult> UpdateUserTask([FromBody] UserTask userTask)
        {
            try
            {
                userTask.UserMaj = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                userTask.userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

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

                string filePath = await _SUserTask.GenerateUserTask(filter);

                await _SUserTask.UpdateHistoGenereExcel(new Export
                {
                    dateFrom = filter.startDate,
                    dateTo = filter.endDate,
                    user_maj = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                });

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                string fileName = Path.GetFileName(filePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);



            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        
        
    }
}
