using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DTO;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ITasksService
    {
        IEnumerable<TasksDto> GetAllTasks();
        TasksDto GetTaskById(int id);
        void CreateTask(TasksDto task);
        void UpdateTask(TasksDto task);
        void DeleteTask(int id);
    }
}
