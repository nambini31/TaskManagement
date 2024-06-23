using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    
        public class LeaveService
        {
            private readonly ILeaveRepository _leaveRepository;

            public LeaveService(ILeaveRepository leaveRepository)
            {
                _leaveRepository = leaveRepository;
            }

            public async Task<List<Leave>> GetAllLeavesAsync()
            {
                return await _leaveRepository.GetAllAsync();
            }

            public async Task<Leave> GetLeaveByIdAsync(int id)
            {
                return await _leaveRepository.GetByIdAsync(id);
            }

            public async Task AddLeaveAsync(Leave leave)
            {
                await _leaveRepository.AddAsync(leave);
            }

            public async Task UpdateLeaveAsync(Leave leave)
            {
                await _leaveRepository.UpdateAsync(leave);
            }

            public async Task DeleteLeaveAsync(int id)
            {
                await _leaveRepository.DeleteAsync(id);
            }
        }
   
}
