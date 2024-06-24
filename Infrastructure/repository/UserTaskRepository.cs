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
            await _db.Database.ExecuteSqlAsync($"SET @userDeleteUserTask = {1}");

            UserTask? Usertask = await _db.UserTask.FirstOrDefaultAsync(u => u.UserTaskId == UsertaskId);

            _db.UserTask.Remove(Usertask);
             await _db.SaveChangesAsync();

        }

        public  async Task<UserTask> GetUserTaskById(int UsertaskId)
        {

            try
            {
                string sql = @"select *, 
                            
                            CASE 
                                WHEN leaveId IS NOT NULL AND leaveId != 0 THEN true 
                                ELSE false 
                            END AS isLeave
                            from
                            usertask WHERE userTaskId = {0}
                                ";

                UserTask data = await _db.UserTask.FromSqlRaw(sql , UsertaskId).FirstAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
        }

        public async Task<IEnumerable<UserTask>> GetUserTask()
        {
            //vrai IEnumerable<UserTask> data = await _db.Usertask.Include(u => u.category).ToListAsync();
            IEnumerable<UserTask> data = await _db.UserTask.ToListAsync();

            return data;
         
        }
        public async Task<IEnumerable<UserTaskVM>> GetUserTasksVM(FiltreUserTask filter)
        {

            string user = (filter.userId == null || filter.userId == "All" ) ?  "" : $"and user.userId = {filter.userId} ";

            try
            {
                string sql = $@"select 
                            usertask.UserTaskId, 
                            usertask.hours, 
                            usertask.date, 
                            tasks.taskId, 
                            CASE 
                                WHEN usertask.leaveId IS NOT NULL  AND usertask.leaveId != 0 THEN true 
                                ELSE false 
                            END AS isLeave,

                            leaves.leaveId, 
                            leaves.reason AS leaveName, 
                            user.userId,
                            user.userName 
                            from 
                            usertask LEFT JOIN tasks on tasks.taskId = usertask.taskId
                            LEFT JOIN leaves on leaves.leaveId = usertask.leaveId LEFT JOIN user on user.userId = usertask.userId 
                            WHERE ( usertask.date BETWEEN @start AND @end ) {user}
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
                    projectId = a.Tasks != null ? a.Tasks.projectId : a.leaveId,
                    projectName = a.Tasks != null ? a.Tasks.project.name : null,
                    taskId = a.taskId,
                    taskName = a.isLeave ? a.Leaves.reason : a.Tasks.name,
                    userId = a.userId,
                    userName = a.User.Username,
                  
                    
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
