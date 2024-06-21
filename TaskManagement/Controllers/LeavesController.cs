using Domain.Entity;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Models.ViewModel;

namespace TaskManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly LeaveService _leaveService;

        public LeavesController(LeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            var leaves = await _leaveService.GetAllLeavesAsync();
            return Ok(leaves);
        }

        [HttpPost]
        public async Task<IActionResult> AddLeave([FromBody] Leave leave)
        {
            if (leave == null || string.IsNullOrWhiteSpace(leave.reason))
            {
                return BadRequest("Invalid input");
            }

            await _leaveService.AddLeaveAsync(leave);
            return Ok(new { success = true });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLeave([FromBody] Leave leave)
        {
            if (leave == null || string.IsNullOrWhiteSpace(leave.reason))
            {
                return BadRequest("Invalid input");
            }

            await _leaveService.UpdateLeaveAsync(leave);
            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            await _leaveService.DeleteLeaveAsync(id);
            return Ok(new { success = true });
        }
    }
}
