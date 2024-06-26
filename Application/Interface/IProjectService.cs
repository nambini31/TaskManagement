using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DTO;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectAsync();
        Task<ProjectDto> GetProjectByIdAsync(int id);
        Task CreateProjectAsync(ProjectDto projectDto);
        Task UpdateProjectAsync(ProjectDto projectDto);
        Task DeleteProjectAsync(int id);

    }
}

