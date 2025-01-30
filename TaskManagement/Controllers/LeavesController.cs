using Domain.DTO;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskManagement.Controllers
{
    //[Authorize]
    public class LeavesController : Controller
    {
        private readonly ILeavesService _leavesService;

        public LeavesController(ILeavesService leavesService)
        {
            _leavesService = leavesService;
        }

        public async Task<IActionResult> Index()
        {
            var leaves = await _leavesService.GetAllLeavesAsync();
            return View(leaves);
        }

        public IActionResult Create()
        {
            return PartialView(new LeaveDto());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveDto leaveDto)
        {
            if (ModelState.IsValid)
            {
                await _leavesService.CreateLeaveAsync(leaveDto);
                return RedirectToAction(nameof(Index));
            }
            return PartialView(leaveDto);
        }

        public async Task<IActionResult> Edit(int id)
        {

            var leave = await _leavesService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return PartialView(leave);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeaveDto leaveDto)
        {
            if (ModelState.IsValid)
            {
                var user_maj = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _leavesService.UpdateLeaveAsync(leaveDto, user_maj);
                return RedirectToAction(nameof(Index));
            }
            return PartialView(leaveDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _leavesService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return PartialView(leave);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user_maj = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _leavesService.DeleteLeaveAsync(id, user_maj);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeaves()
        {
            IEnumerable<LeaveDto> projects = await _leavesService.GetAllLeavesAsync();
            return Json(new { data = projects });
        }

    }
}
