using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
