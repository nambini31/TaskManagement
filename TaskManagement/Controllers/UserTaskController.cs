using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Application.Services;
using Domain.DTO;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Domain.DTO.ViewModels;

namespace TaskManagement.Controllers
{
    [Authorize]
    public class UserTaskController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SUserTaskRepository _SUserTask;
        private readonly UserServiceRepository _UserService;

        public UserTaskController(ILogger<HomeController> logger, SUserTaskRepository _SUserTask, UserServiceRepository _UserService)
        {
            _logger = logger;
            this._SUserTask = _SUserTask;
            this._UserService = _UserService;
        }

        public IActionResult Index()
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
                _logger.LogError(ex, "Erreur lors de la récupération des tâches utilisateur");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserTask model)
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
                    _logger.LogError(ex, "Erreur lors de la sauvegarde de la tâche utilisateur");
                    return Json(new { success = false, error = "Une erreur s'est produite lors de l'enregistrement des modifications. Voir l'exception interne pour plus de détails." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return Json(new { success = false, error = "État du modèle invalide", errors });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserTask(int userTaskId)
        {
            try
            {
                var userConnected = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _SUserTask.DeleteUserTaskById(userTaskId, userConnected);
                return Ok(new { message = "Succès" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la tâche utilisateur");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ModalUserTaskEdit(int userTaskId)
        {
            try
            {
                UserTask userTask = await _SUserTask.GetUserTaskById(userTaskId);
                return PartialView("_modalEdit", userTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la tâche utilisateur pour modification");
                return StatusCode(500, "Erreur interne du serveur");
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
                return Ok(new { message = "Succès" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la tâche utilisateur");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExcelUserTask(FiltreUserTask filter)
        {
            try
            {
                await _SUserTask.GenerateUserTask(filter);
                return Ok(new { message = "Succès" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la génération du fichier Excel pour les tâches utilisateur");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }
    }
}
