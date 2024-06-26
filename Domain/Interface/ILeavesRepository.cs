using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ILeavesRepository
    {
        Task<IEnumerable<Leaves>> GetAllAsync();
        Task<Leaves> GetByIdAsync(int id);
        Task AddAsync(Leaves leaves);
        Task UpdateAsync(Leaves leaves);
        Task DeleteAsync(int id);
    }
}
