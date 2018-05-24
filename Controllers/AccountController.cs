using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Infrastructure;
using ProductControl.BLL.Interfaces;
using ProductControl.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductControl.Controllers
{
    public class AccountController : Controller
    {
        private IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                ApplicationUserDTO userDto = new ApplicationUserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await userService.Authenticate(userDto);
                if (claim == null)
                {
                    return Json("Wrong email or password", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    userDto = await userService.GetUser(userDto);
                    return Json(userDto, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(model);
        }

        public JsonResult Logout()
        {
            AuthenticationManager.SignOut();
            return Json("LogOut");
        }

        [HttpGet]
        public async Task<JsonResult> GetUser(CheckModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                ApplicationUserDTO userDto = new ApplicationUserDTO { Email = model.Email, SecurityStamp = model.SecurityStamp };
                try
                {
                    ApplicationUserDTO user = await userService.GetUserByEmailAndSecurityStamp(userDto);
                    return Json(user, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json("Email or Token wrang", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                ApplicationUserDTO userDto = new ApplicationUserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Role = "user"
                };

                OperationResult result = await userService.Create(userDto);
                return Json(result.Result);
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> RegisterCourier(RegisterCourierModel model)
        {
            await SetInitialDataAsync();
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            if (!Validator.TryValidateObject(model, context, results, true))
            {
                return Json("Bad data");
            }
            ApplicationUserDTO checkAdmin = new ApplicationUserDTO
            {
                Email = model.EmailAdmin,
                SecurityStamp = model.SecurityStamp
            };
            try
            {
                ApplicationUserDTO admin = await userService.GetUserByEmailAndSecurityStamp(checkAdmin);
                if (admin.Role != "admin")
                {
                    return Json("Only Admin can add courier");
                }
                ApplicationUserDTO userDto = new ApplicationUserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Role = "courier"
                };

                OperationResult result = await userService.Create(userDto);
                return Json(result.Result);
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }


        private async Task SetInitialDataAsync()
        {
            await userService.SetInitialData(new ApplicationUserDTO
            {
                Email = "roket98@gmail.com",
                Password = "qwe123!",
                FirstName = "Vitalii",
                SecondName = "Batuchenko",
                Role = "admin",
            }, new List<string> { "user", "admin", "courier" });
        }
    }
}