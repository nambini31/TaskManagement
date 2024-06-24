using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITasksRepository
    {
        Task<IEnumerable<Tasks>> GetAllAsync();
        Task<Tasks> GetByIdAsync(int id);
        Task CreateAsync(Tasks tasks);
        Task UpdateAsync(Tasks tasks);
        Task DeleteAsync(int id);
    }
}
