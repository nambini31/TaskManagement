using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;
using Domain.DTO.ViewModels;
using System.Threading.Tasks.Dataflow;
using MySqlConnector;

namespace Infrastructure.Repository
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly ApplicationDbContext _db;
      
        public UserTaskRepository(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task AddUserTask(UserTask Usertask)
        {
            
           await _db.UserTask.AddAsync(Usertask);
            await _db.SaveChangesAsync();

        }

        public async Task DeleteUserTaskById(int UsertaskId)
        {
            var Usertask = await _db.UserTask.FirstOrDefaultAsync(u => u.UserTaskId == UsertaskId);

            _db.UserTask.Remove(Usertask);
             await _db.SaveChangesAsync();

        }

        public  async Task<UserTask> GetUserTaskById(int UsertaskId)
        {
            UserTask? art = await _db.UserTask.FirstOrDefaultAsync(a => a.UserTaskId == UsertaskId );

            return art;
        }

        public async Task<IEnumerable<UserTask>> GetUserTask()
        {
            //vrai IEnumerable<UserTask> data = await _db.Usertask.Include(u => u.category).ToListAsync();
            IEnumerable<UserTask> data = await _db.UserTask.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<UserTaskVM>> GetUserTasksVM(FiltreUserTask filter)
        {

            try
            {
                string sql = @"select 
                            usertask.UserTaskId, 
                            usertask.hours, 
                            usertask.date, 
                            tasks.taskId, 
                            CASE 
                                WHEN leaves.leaveId IS NOT NULL THEN leaves.reason 
                                ELSE tasks.name 
                            END AS taskName, 
                            leaves.leaveId, 
                            leaves.reason AS leaveName, 
                            user.userId,
                            user.userName 
                            from 
                            usertask join tasks on tasks.taskId = usertask.taskId 
                            join leaves on leaves.leaveId = usertask.leaveId join user on user.userId = usertask.userId 
                            WHERE usertask.date BETWEEN @start AND @end
                                ";

                IEnumerable<UserTaskVM> data = await _db.UserTask.FromSqlRaw(sql,
                    new MySqlParameter(sql="@start", filter.startDate.ToString("yyyy-MM-dd")),
                    new MySqlParameter(sql="@end", filter.endDate.ToString("yyyy-MM-dd"))).Select(a =>
                new UserTaskVM
                {
                    UserTaskId = a.UserTaskId,
                    datetime = a.date,
                    hours = a.hours,
                    leaveId = a.leaveId,
                    projectId = a.Tasks.projectId,
                    projectName = a.Tasks.Project.name,
                    taskId = a.taskId,
                    taskName = a.Tasks.name,
                    userId = a.userId,
                    userName = a.User.Username
                }
                ).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public  async Task UpdateUserTask(UserTask Usertask)
    {
             _db.UserTask.Update(Usertask);

             await _db.SaveChangesAsync();

        }
    }
}
