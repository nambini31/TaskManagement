using Application.Services;
using Domain.Entity;
using Domain.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Models.AccountVM;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.Controllers
{
    public class AccountController : Controller
    {
       private readonly UserServiceRepository _userService;

        public AccountController(UserServiceRepository userService)
        {
            _userService = userService;
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View("User");
        }

        //-- get all User for data table
        [HttpPost]
        public IActionResult GetAll()
        {
            var users = _userService.GetUser();
            return Json(users);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)  
            {
                _userService.RegisterUser(model.Name, model.Surname, model.Username, model.Password, model.Email, model.Role);
                return RedirectToAction("Index", "Account");
            }

            return View("~/Views/Account/User.cshtml", model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            TempData["appName"] = "GesPro";
            return View("~/Views/Auth/login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = _userService.Authenticate(model.Username, model.Password);
                if (user != null)
                {
                    var role = _userService.GetRoleByUserId(user.UserId);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                    };
                    if (role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                   
                    
                    return RedirectToAction("Index", "Account");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("~/Views/Auth/login.cshtml", model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
