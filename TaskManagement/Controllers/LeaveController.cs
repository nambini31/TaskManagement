using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
