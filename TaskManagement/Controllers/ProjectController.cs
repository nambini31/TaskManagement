using Application.Interface;
using Application.Services;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskManagement.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            var projects = _projectService.GetAllProjectAsync();
            return Json(new { data = projects });
        }

        public async Task<IActionResult> Index()
        {
            @ViewData["titrePage"] = "Project Management";
            var projects = await _projectService.GetAllProjectAsync();
            return View(projects);
        }

        public IActionResult Create()
        {
            return PartialView(new ProjectDto());
        }


        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                await _projectService.CreateProjectAsync(projectDto);
                return RedirectToAction(nameof(Index));
            }
            return PartialView(projectDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return PartialView(project);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(ProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                var user_maj = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _projectService.UpdateProjectAsync(projectDto, user_maj);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("Edit", projectDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return PartialView(project);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user_maj = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _projectService.DeleteProjectAsync(id, user_maj);
            return RedirectToAction(nameof(Index));
        }
    }
}
