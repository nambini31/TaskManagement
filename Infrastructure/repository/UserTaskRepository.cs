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
using System.Security.Claims;

namespace Infrastructure.Repository
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly ApplicationDbContext _db;
      
        public UserTaskRepository(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task AddUserTask(List<UserTask> usertask)
        {

            foreach (var item in usertask)
            {
                
                await _db.UserTask.AddAsync(item);
                
            }

            await _db.SaveChangesAsync();

        }

        public async Task DeleteUserTaskById(int UsertaskId, int userConnected)
        {
            //await _db.Database.ExecuteSqlAsync($"SET @userDeleteUserTask = {1}");

            UserTask? Usertask = await _db.UserTask.Select(a => new UserTask
            {

                userId = a.userId,
                UserTaskId = a.UserTaskId,
                date = a.date,
                hours = a.hours,
                taskId = a.taskId,
                leaveId = a.leaveId

            }).FirstOrDefaultAsync(u => u.UserTaskId == UsertaskId);

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.Database.ExecuteSqlRaw("SET @userConnected = {0}", userConnected);


                    _db.UserTask.Remove(Usertask);
                    await _db.SaveChangesAsync();

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public  async Task<UserTask> GetUserTaskById(int UsertaskId)
        {

            try
            {
                string sql = @"select *
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
            

            string user =   filter.userId == null    ? "and UserTask.userId != 0" : $"and user.userId  IN ({string.Join(',', filter.userId)}) ";


            if (true)
            {
                
            }

            try
            {
                string sql = $@"select 
                            usertask.UserTaskId, 
                            usertask.hours, 
                            usertask.date, 
                            tasks.taskId, 
                            isLeave,
                            leaves.leaveId, 
                            leaves.reason AS leaveName, 
                            user.userId,
                            user.userName 
                            from 
                            usertask LEFT JOIN tasks on tasks.taskId = usertask.taskId
                            LEFT JOIN leaves on leaves.leaveId = usertask.leaveId LEFT JOIN user on user.userId = usertask.userId 
                            WHERE ( date(usertask.date) BETWEEN @start AND @end ) {user}
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
        public async Task<IEnumerable<UserTaskVM>> GetUserTasksByUsersVM(FiltreUserTask filter)
        {

            string user = filter.userId == null ? "and UserTask.userId != 0" : $"and user.userId  IN ({string.Join(',', filter.userId)}) ";

            try
            {
                string sql = $@"SELECT 
                SUM(usertask.hours) AS hours, 
                usertask.date, 
                tasks.taskId, 
                isLeave,
                CASE 
                    WHEN usertask.leaveId IS NOT NULL AND usertask.leaveId != 0 THEN leaves.reason 
                    ELSE tasks.name 
                END AS taskName,
                user.userName ,
                leaves.leaveId,
                user.userId 
            FROM 
                usertask 
                LEFT JOIN tasks ON tasks.taskId = usertask.taskId
                LEFT JOIN leaves ON leaves.leaveId = usertask.leaveId 
                LEFT JOIN user ON user.userId = usertask.userId 
            WHERE ( date(usertask.date) BETWEEN @start AND @end ) {user}
            GROUP BY 
                user.userId, 
                taskName 
                            
                                ";

                IEnumerable<UserTaskVM> data = await _db.UserTask.FromSqlRaw(sql,
                    new MySqlParameter(sql="@start", filter.startDate.ToString("yyyy-MM-dd")),
                    new MySqlParameter(sql="@end", filter.endDate.ToString("yyyy-MM-dd"))).Select(a =>
                new UserTaskVM
                {
                    datetime = a.date,
                    hours = a.hours,
                    taskName = a.isLeave ? a.Leaves.reason : (a.Tasks.project.name +" - "+ a.Tasks.name),
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
        
        public async Task<IEnumerable<UserTaskVM>> GetUserTasksForTwoDate(FiltreUserTask filter)
        {

            string user = filter.userId == null ? "and UserTask.userId != 0" : $"and user.userId  IN ({string.Join(',', filter.userId)}) ";

            try
            {
                string sql = $@"SELECT hours, 
                usertask.date, 
                tasks.taskId, 
                isLeave,
                CASE 
                    WHEN usertask.leaveId IS NOT NULL AND usertask.leaveId != 0 THEN leaves.reason 
                    ELSE tasks.name 
                END AS taskName,
                user.userName ,
                leaves.leaveId,
                user.userId 
            FROM 
                usertask 
                LEFT JOIN tasks ON tasks.taskId = usertask.taskId
                LEFT JOIN leaves ON leaves.leaveId = usertask.leaveId 
                LEFT JOIN user ON user.userId = usertask.userId 
            WHERE ( date(usertask.date) BETWEEN @start AND @end ) {user}
            GROUP BY 
                user.userId                             
                                ";

                IEnumerable<UserTaskVM> data = await _db.UserTask.FromSqlRaw(sql,
                    new MySqlParameter(sql="@start", filter.startDate.ToString("yyyy-MM-dd")),
                    new MySqlParameter(sql="@end", filter.endDate.ToString("yyyy-MM-dd"))).Select(a =>
                new UserTaskVM
                {

                    datetime = a.date,
                    hours = a.hours,
                    taskName = a.isLeave ? a.Leaves.reason : (a.Tasks.project.name +" - "+ a.Tasks.name),
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
        public async Task<IEnumerable<UserTaskVM>> GetUserTasksGrouperVM(FiltreUserTask filter)
        {

            string user = filter.userId == null ? "and UserTask.userId != 0" : $"and user.userId  IN ({string.Join(',', filter.userId)}) ";

            try
            {
                string sql = $@"SELECT hours, 
                usertask.date, 
                tasks.taskId, 
                isLeave,
                CASE 
                    WHEN usertask.isLeave THEN leaves.reason 
                    ELSE tasks.name 
                END AS taskName,
                user.userName ,
                leaves.leaveId,
                user.userId 
            FROM 
                usertask 
                LEFT JOIN tasks ON tasks.taskId = usertask.taskId
                LEFT JOIN leaves ON leaves.leaveId = usertask.leaveId 
                LEFT JOIN user ON user.userId = usertask.userId 
            WHERE ( date(usertask.date) BETWEEN @start AND @end ) {user}
            GROUP BY 
                taskName";

                IEnumerable<UserTaskVM> data = await _db.UserTask.FromSqlRaw(sql,
                    new MySqlParameter(sql="@start", filter.startDate.ToString("yyyy-MM-dd")),
                    new MySqlParameter(sql="@end", filter.endDate.ToString("yyyy-MM-dd"))).Select(a =>
                new UserTaskVM
                {

                    datetime = a.date,
                    hours = a.hours,
                    taskName = a.isLeave ? a.Leaves.reason : (a.Tasks.project.name +" - "+ a.Tasks.name),
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
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //denifinition du session pour utiliser dans trigger
                    _db.Database.ExecuteSqlRaw("SET @userConnected = {0}", Usertask.UserMaj);

                    _db.UserTask.Update(Usertask);
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        public  async Task UpdateHistoGenereExcel(Export filter)
        {
            
                try
                {
                    

                    await _db.Exportexcel_histo.AddAsync(filter);
                    await _db.SaveChangesAsync();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            

        }
    }
}
