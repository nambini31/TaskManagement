using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ILeaveRepository
    {
        Task<List<Leave>> GetAllAsync();
        Task<Leave> GetByIdAsync(int id);
        Task AddAsync(Leave leave);
        Task UpdateAsync(Leave leave);
        Task DeleteAsync(int id);
    }
}
