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
using Domain.DTO;
using Newtonsoft.Json;

namespace TaskManagement.Controllers
{

    //[Authorize]
    public class UserController : Controller
    {
       private readonly UserServiceRepository _userService;

        public UserController(UserServiceRepository userService)
        {
            _userService = userService;
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            @ViewData["titrePage"] = "User Management";
            return View();
        }

        //-- get all User for datatable
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            return Json(user);
        }
        //----------------------------------

        //-- get all User for datatable with role
        //[Authorize]
        [HttpPost]
        public IActionResult GetAllUser()
        {
            IEnumerable<UserListWithRole>  users ;

            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                users = new List<UserListWithRole> { _userService.GetUserById(currentUserId) };
            }
            else
            {

                 users = _userService.GetUser();
            
            }

            return Json(users);
        }
        //----------------------------------

        //[Authorize]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[Authorize]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user != null)
            {
                return RedirectToAction("Create", "UserTask");
            }
            else
            {
                return View("Login", model);
            }

            //try
            //{

            //    if (ModelState.IsValid)
            //    {
            //        var user = _userService.Authenticate(model.Username, model.Password);
            //        //set Session

            //        var session = JsonConvert.SerializeObject(user);
            //        HttpContext.Session.SetString("User", session);
            //        if (user != null)
            //        {
            //            var role = _userService.GetRoleByUserId(user.UserId);                        
            //            //var claims = new List<Claim>
            //            //{
            //            //    new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
            //            //    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            //            //};

            //            if (role != null)
            //            {
            //                //claims.Add(new Claim(ClaimTypes.Role, role.Name));
            //                HttpContext.Session.SetString("RoleName", role.Name);
            //            }

            //            //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            //            //if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            //            //{
            //            //    return Redirect(ReturnUrl);
            //            //}
            //            //else
            //            //{
            //            //    // Redirect to default page
            //            //    //return RedirectToAction("Index", "Home");
            //            //}
            //                return RedirectToAction("Create", "UserTask");

            //        }

            //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //    }

            //    return View("Login", model);
            //}
            //catch (Exception ex) {
            //    throw ex;
            //}
        }

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }


        //-- Update User ----
        //[Authorize]
        public IActionResult Update(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Récupère l'ID de l'utilisateur connecté
                    var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    _userService.UpdateUser(model.UserId, model.Name, model.Surname, model.Username, model.Password, model.Email, model.Role, currentUserId);
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
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "Invalid user ID." });
            }
            try
            {
                // Récupère l'ID de l'utilisateur connecté
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var userToDelete = _userService.GetUserWithoutRole(userId);
                _userService.DeleteUserService(userToDelete, currentUserId);
                return Json(new { success = true, message = "Successfuly !" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Can\'t Delete ! {ex.Message}" });
            }
        }
        //---------------------------------------

        //-- Acces denied --
        public IActionResult AccesDenied()
        {
            TempData["messageLogin"] = "You are not authorized to access this page";
            return RedirectToAction("Login");      
        }
        //--------------------------------------------

        //-- Must log in --
        public IActionResult MustLogin()
        {
            TempData["messageLogin"] = "You must log in";
            return RedirectToAction("Login");
        }
        //--------------------------------------------

    }
}
