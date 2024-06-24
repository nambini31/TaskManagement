using Application.Services;
using Domain.Entity;
using Domain.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Domain.DTO.ViewModels.UserVM;
using static Application.Services.UserServiceRepository;

namespace TaskManagement.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
       private readonly UserServiceRepository _userService;

        public UserController(UserServiceRepository userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            @ViewData["titrePage"] = "User Management";
            return View();
        }

        //-- get all User for datatable
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            return Json(user);
        }
        //----------------------------------

        //-- get all User for datatable with role
        [HttpPost]
        public IActionResult GetAllUser()
        {
            var users = _userService.GetUser();
            return Json(users);
        }
        //----------------------------------

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
                try
                {
                    // Récupère l'ID de l'utilisateur connecté
                    //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

                    _userService.RegisterUser(model.Name, model.Surname, model.Username, model.Password, model.Email, model.Role);
                    return Json(new { success = true, message = "Successfuly" });
                }
                catch (AfficheException ex)
                {
                    return Json(new { success = false, message = ex.Message });
                } catch (Exception ex) 
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            //return les erreur de validation
            var erreurValidation = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, erreurValidation });
        }


        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            TempData["appName"] = "GesPro";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl = null)
        {

            if (ModelState.IsValid)
            {
                var user = _userService.Authenticate(model.Username, model.Password);
                if (user != null)
                {
                    var role = _userService.GetRoleByUserId(user.UserId);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                    };
                    if (role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        // Redirect to default page
                        //return RedirectToAction("Index", "Home");
                        return RedirectToAction("Index", "User");
                    }

                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("Login", model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        //-- Update User ----
        public IActionResult Update(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.UpdateUser(model.UserId, model.Name, model.Surname, model.Username, model.Password, model.Email, model.Role);
                    //return RedirectToAction("Index", "User");
                    return Json(new { success = true, message = "Successfuly" });

                }
                catch (AfficheException ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            //gerer les erreurs de navigaion
            var erreurValidation = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Json(new { success = false, erreurValidation });

        }
        //--------------------------------------------------


        //-- Delete User ------
        // Action POST pour supprimer un utilisateur
        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "Invalid user ID." });
            }
            //try
            //{
                var userToDelete = _userService.GetUserWithoutRole(userId);
                _userService.DeleteUserService(userToDelete);
                return Json(new { success = true, message = "Successfuly !" });
            //}
            //catch(Exception ex)
            //{
            //    return Json(new { success = false, message = $"Can\'t Delete ! {ex.Message}" });
            //}
        }
        //---------------------------------------

        //-- Acces denied --
        public IActionResult AccesDenied()
        {
            TempData["messageLogin"] = "You are not authorized to access this page";
            return RedirectToAction("Login");      
        }
        //--------------------------------------------

    }
}
