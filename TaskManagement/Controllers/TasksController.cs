using Application.Interface;
using Application.Services;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{

    public class TasksController : Controller
    {
        private readonly ITasksService _tasksService;
        private readonly IProjectService _projectService;

        public TasksController(ITasksService tasksService, IProjectService projectService)
        {
            _tasksService = tasksService;
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var tasks = _tasksService.GetAllTasks();
            return Json(new { data = tasks });
        }

        [HttpPost]
        public IActionResult Create([FromBody] TasksDto task)
        {
            if (ModelState.IsValid)
            {
                _tasksService.CreateTask(task);
                return Json(new { success = true, message = "Task created successfully" });
            }
            return Json(new { success = false, message = "Error while creating task" });
        }


        [HttpGet]
        public IActionResult GetEdit(int id)
        {
            var task = _tasksService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }

            var projects = _projectService.GetAllProjects();
            ViewBag.Projects = projects;

            var taskDto = new TasksDto
            {
                taskId = task.taskId,
                name = task.name,
                projectId = task.projectId
            };

            return View("Edit", taskDto);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] TasksDto task)
        {
            if (ModelState.IsValid)
            {
                _tasksService.UpdateTask(task);
                return Json(new { success = true, message = "Task updated successfully" });
            }
            return Json(new { success = false, message = "Error while updating task" });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _tasksService.DeleteTask(id);
            return Json(new { success = true, message = "Task deleted successfully" });
        }
    }
}

