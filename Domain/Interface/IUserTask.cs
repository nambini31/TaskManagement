using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ViewModels;
using Domain.Entity;

namespace Domain.Interface
{
    public interface IUserTask
    {
        Task<IEnumerable<UserTask>> GetUserTask();
        Task<IEnumerable<UserTaskVM>> GetUserTasksVM();
        Task<UserTask> GetUserTaskById(int articleId);
        Task AddUserTask(UserTask article);
        Task UpdateUserTask(UserTask article);
        Task DeleteUserTaskById(int articleId);

    }
}
