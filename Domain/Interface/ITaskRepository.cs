using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Task>> GetTasks();
        Task<Task> GetTaskId(int TaskId);
        Task AddTask(Task Task);
        Task UpdateTask(Task Task);
        Task DeleteTask(int TaskId);
    }
}
