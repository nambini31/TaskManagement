using Domain.DTO;
using Domain.Entity;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LeavesService : ILeavesService
    {
        private readonly ILeavesRepository _leavesRepository;

        public LeavesService(ILeavesRepository leavesRepository)
        {
            _leavesRepository = leavesRepository;
        }

        public async Task<IEnumerable<LeaveDto>> GetAllLeavesAsync()
        {
            var leaves = await _leavesRepository.GetAllAsync();
            return leaves.Select(l => new LeaveDto
            {
                leaveId = l.leaveId,
                reason = l.reason
            });
        }

        public async Task<LeaveDto> GetLeaveByIdAsync(int id)
        {
            var leave = await _leavesRepository.GetByIdAsync(id);
            if (leave == null)
                return null;

            return new LeaveDto
            {
                leaveId = leave.leaveId,
                reason = leave.reason
              
            };
        }

        public async Task CreateLeaveAsync(LeaveDto leaveDto)
        {
            var leave = new Leaves
            {
                leaveId = leaveDto.leaveId,
                reason = leaveDto.reason
               
            };

            await _leavesRepository.AddAsync(leave);
        }

        public async Task UpdateLeaveAsync(LeaveDto leaveDto)
        {
            var leave = await _leavesRepository.GetByIdAsync(leaveDto.leaveId);
            if (leave == null)
                throw new ArgumentException("Leave not found");

            leave.reason = leaveDto.reason;
            await _leavesRepository.UpdateAsync(leave);
        }

        public async Task DeleteLeaveAsync(int id)
        {
            await _leavesRepository.DeleteAsync(id);
        }
    }
}
