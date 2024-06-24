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
using Infrastructure.Repository;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectAsync()
        {
            var project = await _projectRepository.GetAllAsync();
            return project.Select(p => new ProjectDto
            {
                projectId = p.projectId,
                name = p.name,
                description = p.description
                
            });
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return null;

            return new ProjectDto
            {
                projectId = project.projectId,
                name = project.name,
                description = project.description

            };
        }



        public async Task CreateProjectAsync(ProjectDto projectDto)
        {
            var project = new Project
            {
                projectId = projectDto.projectId,
                name = projectDto.name,
                description = projectDto.description

            };

            await _projectRepository.AddAsync(project);
        }


        public async Task UpdateProjectAsync(ProjectDto projectDto)
        {
            var project = await _projectRepository.GetByIdAsync(projectDto.projectId);
            if (project == null)
                throw new ArgumentException("Leave not found");

            project.name = projectDto.name;
            project.description = projectDto.description;
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            await _projectRepository.DeleteAsync(id);
        }
    }
}
