using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ViewModels;
using Domain.Entity;
using Domain.Interface;

namespace Application.Service
{
    public class SUserTaskRepository
    {
        private readonly IUserTaskRepository IUserTask;
        public SUserTaskRepository(IUserTaskRepository IUserTask) {

            this.IUserTask = IUserTask;

        }
        public async Task AddUserTask(UserTask article)
        {
             await IUserTask.AddUserTask(article);
        }

        public async Task DeleteUserTaskById(int articleId)
        {
           await IUserTask.DeleteUserTaskById(articleId);
        }

        public  async Task<UserTask> GetUserTaskById(int articleId)
        {
           return await IUserTask.GetUserTaskById(articleId);
        }

        public async Task<IEnumerable<UserTask>> GetUserTask()
        {
            return  await IUserTask.GetUserTask();
        } 
        public async Task<IEnumerable<UserTaskVM>> GetUserTaskVM(FiltreUserTask filter)
        {
            return await IUserTask.GetUserTasksVM(filter);
        }

        public async Task UpdateUserTask(UserTask userTask)
        {
            await IUserTask.UpdateUserTask(userTask);
        }
    }
}
