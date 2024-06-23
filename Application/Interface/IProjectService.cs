using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    class IProjectService
    {
        IEnumerable<ProjectDto> GetAllProjects();
        ProjectDto GetProjectById(int id);
        void CreateProject(ProjectDto project);
        void UpdateProject(ProjectDto project);
        void DeleteProject(int id);
    }
}
