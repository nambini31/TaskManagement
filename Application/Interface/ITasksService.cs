
using Domain.DTO;

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
    }
}
