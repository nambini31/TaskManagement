using Application.Services;
using Domain.DTO;
using Domain.Entity;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{

    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            var projects = _projectService.GetAllProjects();
            return Json(new { data = projects });
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectDto project)
        {
            if (ModelState.IsValid)
            {
                _projectService.CreateProject(project);
                return Json(new { success = true, message = "Project created successfully" });
            }
            return Json(new { success = false, message = "Error while creating project" });
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return Json(project);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ProjectDto project)
        {
            if (ModelState.IsValid)
            {
                _projectService.UpdateProject(project);
                return Json(new { success = true, message = "Project updated successfully" });
            }
            return Json(new { success = false, message = "Error while updating project" });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _projectService.DeleteProject(id);
            return Json(new { success = true, message = "Project deleted successfully" });
        }
    }
}

    
