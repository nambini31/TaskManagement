using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Application.Services;
using Domain.DTO;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Domain.DTO.ViewModels;
using System.Net.Mail;
using Application.Services.Mail;

namespace TaskManagement.Controllers
{
   [Authorize]
    public class UserTaskController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SUserTaskRepository _SUserTask;
        private readonly UserServiceRepository _UserService;
        private readonly ISendMailService _sendMailService;
        private readonly IFileViewToStringforEmailService _HtmlFileViewToStringforEmail;


        public UserTaskController(ILogger<HomeController> logger, SUserTaskRepository _SUserTask, UserServiceRepository _UserService, ISendMailService sendMailService,
            IFileViewToStringforEmailService htmlFileViewToStringforEmail)
        {
            _logger = logger;
            this._SUserTask = _SUserTask;
            this._UserService = _UserService;
            _sendMailService = sendMailService;
            _sendMailService = sendMailService;
            _HtmlFileViewToStringforEmail = htmlFileViewToStringforEmail;
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
                if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
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
            ViewData["titrePage"] = "Time Tracking";
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody , Bind(include: "taskId,leaveId,projectId,date,hours")] List<UserTask> model)
        {
            
                try
                {
                    var userConnected = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    foreach (var item in model)
                    {

                        item.userId = userConnected;      

                    }

                    await _SUserTask.AddUserTask(model);

                    return Json(new { success = true });
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
                var destemails = new[] { "jmbolaheriniaiko@saimltd.mu", "assistance@saimltd.mu" };
                string subject = $"TIMELINE : {filter.startDate} TO {filter.endDate}";
                var content = _HtmlFileViewToStringforEmail
                    .GetHtmlFileContent("timeline.html");
                //.Replace("-_-", "Le " + formattedDate)
                //.Replace("-Commande-", $"Une facture Collectivité a été générée.<a href='{Url + numCommande}'>Voir la commande</a>")
                //.Replace("dans le fichier Excel ci-joint.", "dans le fichier pdf ci-joint.");
                var destemailsCC = new List<string>();
                var attachments = new List<string>
                {
                    filePath
                };
                _sendMailService.sendMail(destemails, subject, content, true, destemailsCC, attachments);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        
    }
}
