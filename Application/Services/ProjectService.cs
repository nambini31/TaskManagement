using Domain.DTO;
using Domain.Entity;
using Domain.Interface;
using Application.Interface;
using Infrastructure.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IEnumerable<ProjectDto> GetAllProjects()
        {
            return _projectRepository.GetAll().Select(p => new ProjectDto
            {
                projectId = p.projectId,
                name = p.name,
                description = p.description
            });
        }

        public ProjectDto GetProjectById(int id)
        {
            var project = _projectRepository.GetById(id);
            return new ProjectDto
            {
                projectId = project.projectId,
                name = project.name,
                description = project.description
            };
        }

        public void CreateProject(ProjectDto project)
        {
            var entity = new Project
            {
                name = project.name,
                description = project.description
            };
            _projectRepository.Create(entity);
        }

        public void UpdateProject(ProjectDto project)
        {
            var entity = _projectRepository.GetById(project.projectId);
            entity.name = project.name;
            entity.description = project.description;
            _projectRepository.Update(entity);
        }

        public void DeleteProject(int id)
        {
            _projectRepository.Delete(id);
        }
    }
}
