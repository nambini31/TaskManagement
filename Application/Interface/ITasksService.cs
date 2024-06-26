using Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Interface
{
    public interface ITasksService
    {
        Task<IEnumerable<TasksDto>> GetAllTasksAsync();
        IEnumerable<Tasks> GetTaskByIdProject(int id);
        Task<TasksDto> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(TasksDto tasksDto);
        Task UpdateTaskAsync(TasksDto tasksDto, int user_maj);
        Task DeleteTaskAsync(int id, int user_maj);
    }
}
