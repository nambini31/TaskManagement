using Application.Interface;
using Domain.DTO;
using Domain.DTO.ViewModels;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            IEnumerable<TasksDto> tasks = await _tasksService.GetAllTasksAsync();
            IEnumerable<ProjectDto> projects = await _projectService.GetAllProjectAsync();

            TasksVM tasksVM = new TasksVM
            {
                ProjectList = projects,
                TasksList = tasks,

            };

            return View(tasksVM);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _tasksService.GetAllTasksAsync();
            var projects = await _projectService.GetAllProjectAsync();

            if (projects == null)
            {

                return RedirectToAction("Index");
            }

            ViewBag.Projects = projects;
            return View();
            return Json(new { data = tasks });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var projects = await _projectService.GetAllProjectAsync();

                if (projects == null)
                {
                   
                    return RedirectToAction("Index");
                }

                ViewBag.Projects = projects;
                return View();
            }
            catch (Exception ex)
            {
                // Gérer l'exception comme vous le souhaitez
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TasksDto task)
        {
            if (ModelState.IsValid)
            {
                await _tasksService.CreateTaskAsync(task);
                return Json(new { success = true, message = "Task created successfully" });
            }
            return Json(new { success = false, message = "Error while creating task" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var task = await _tasksService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                var projects = await _projectService.GetAllProjectAsync();

                // Vérifiez si projects est null ici pour éviter les exceptions
                if (projects == null)
                {
                    
                    return RedirectToAction("Index");
                }

                ViewBag.Projects = projects;

                var taskDto = new TasksDto
                {
                    taskId = task.taskId,
                    name = task.name,
                    projectId = task.projectId
                };

                return View(taskDto);
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TasksDto task)
        {
            if (ModelState.IsValid)
            {
                await _tasksService.UpdateTaskAsync(task);
                return Json(new { success = true, message = "Task updated successfully" });
            }
            return Json(new { success = false, message = "Error while updating task" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _tasksService.DeleteTaskAsync(id);
            return Json(new { success = true, message = "Task deleted successfully" });
        }

        [HttpPost]
        public IActionResult GetTaskByIdProject(int projectId)
        {
            var tasks = _tasksService.GetTaskByIdProject(projectId);
            return Json( tasks );
        }
    }
}
