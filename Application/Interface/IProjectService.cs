
using Domain.DTO;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IProjectService
    {
        IEnumerable<ProjectDto> GetAllProjects();
        ProjectDto GetProjectById(int id);
        void CreateProject(ProjectDto project);
        void UpdateProject(ProjectDto project);
        void DeleteProject(int id);
    }
}

