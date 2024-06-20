using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class UserTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
