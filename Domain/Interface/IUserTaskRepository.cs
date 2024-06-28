using Domain.DTO.ViewModels;
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
        Task<IEnumerable<UserTask>> GetUserTask();
        Task<IEnumerable<UserTaskVM>> GetUserTasksVM(FiltreUserTask filter);
        Task<IEnumerable<UserTaskVM>> GetUserTasksByUsersVM(FiltreUserTask filter);
        Task<IEnumerable<UserTaskVM>> GetUserTasksForTwoDate(FiltreUserTask filter);
        Task<IEnumerable<UserTaskVM>> GetUserTasksGrouperVM(FiltreUserTask filter);

        Task<UserTask> GetUserTaskById(int articleId);
        Task UpdateHistoGenereExcel(Export filter);
        Task AddUserTask(List<UserTask> article);
        Task UpdateUserTask(UserTask article);
        Task DeleteUserTaskById(int articleId, int userConnected);

    }
}
