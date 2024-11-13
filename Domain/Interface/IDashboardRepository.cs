using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Tasks>> GetAllAsync();
        IEnumerable<Tasks> GetTaskByIdProject(int id);
        Task<Tasks> GetByIdAsync(int id);
        Task CreateAsync(Tasks tasks);
        Task UpdateAsync(Tasks tasks, int user_maj);
        Task DeleteAsync(int id, int user_maj);
    }
}
