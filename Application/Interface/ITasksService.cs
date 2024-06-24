using Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Interface
{
    public interface ITasksService
    {
        IEnumerable<TasksDto> GetAllTasks();
        IEnumerable<Tasks> GetTaskByIdProject(int id);
        TasksDto GetTaskById(int id);
        void CreateTask(TasksDto task);
        void UpdateTask(TasksDto task);
        void DeleteTask(int id);
        Task<IEnumerable<TasksDto>> GetAllTasksAsync();
        Task<TasksDto> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(TasksDto tasksDto);
        Task UpdateTaskAsync(TasksDto tasksDto);
        Task DeleteTaskAsync(int id);
    }
}
