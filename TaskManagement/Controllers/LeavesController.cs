using Domain.DTO;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TaskManagement.Controllers
{
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
                await _leavesService.UpdateLeaveAsync(leaveDto);
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
            await _leavesService.DeleteLeaveAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
