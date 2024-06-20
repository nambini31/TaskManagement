using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interface
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> GetProjectId(int ProjectId);
        Task AddProject(Project Project);
        Task UpdateProject(Leave Leave);
        Task DeleteProject(int ProjectId);
    }
}
