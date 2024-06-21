using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;


namespace Domain.Interface
{
    public interface IUserTaskRepository
    {
        Task<IEnumerable<UserTask>> GetUserTasks();
        Task<UserTask> GetUserTaskById(int UserTaskId);
        Task AddUserTask(UserTask UserTask);
        Task UpdateUserTask(UserTask UserTask);
        Task DeleteUserTask(int UserTaskId);
    }
}
