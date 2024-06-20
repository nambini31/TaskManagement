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

namespace Infrastructure.Repository
{
    public class UserTaskRepository : IUserTask
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
        public async Task<IEnumerable<UserTaskVM>> GetUserTasksVM()
        {
            //vrai IEnumerable<UserTask> data = await _db.Usertask.Include(u => u.category).ToListAsync();

            string sql = @"select 
                            usertask.UserTaskId , 
                            usertask.hours , 
                            usertask.date , 
                            task.taskId , 
                            task.name taskName , 
                            leaves.leaveId , 
                            leaves.reason leaveName , 
                            user.userId 
                            from 
                            usertask join task on task.taskId = usertask.taskId 
                            join leaves on leaves.leaveId = usertask.leaveId ";

            IEnumerable<UserTaskVM> data = await _db.UserTask.FromSqlRaw(sql).Select(a => 
            new UserTaskVM
{
                datetime = a.datetime,
                hours = a.hours,
                leaveId = a.leaveId,
                projectId = a.
            }
            ).ToListAsync();

            return;
        }

        public  async Task UpdateUserTask(UserTask Usertask)
    {
             _db.UserTask.Update(Usertask);

             await _db.SaveChangesAsync();

        }
    }
}
