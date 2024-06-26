using Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ILeavesService
    {
        Task<IEnumerable<LeaveDto>> GetAllLeavesAsync();
        Task<LeaveDto> GetLeaveByIdAsync(int id);
        Task CreateLeaveAsync(LeaveDto leaveDto);
        Task UpdateLeaveAsync(LeaveDto leaveDto, int user_maj);
        Task DeleteLeaveAsync(int id, int user_maj);
    }
}
