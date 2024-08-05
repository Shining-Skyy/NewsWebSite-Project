using Domain.Users;
using Management.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User newUser = new User()
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var result = _userManager.CreateAsync(newUser, model.Password).Result;
            if (result.Succeeded)
            {
                return Redirect("Index");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }

            return View(model);
        }

        public IActionResult Login(string returnUtl = "/")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUtl,
            });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View(model);
            }

            _signInManager.SignOutAsync();
            var result = _signInManager.PasswordSignInAsync(user, model.Password, model.IsPersistent, true).Result;

            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl);
            }
            //if (result.RequiresTwoFactor)
            //{
            //
            //}

            ModelState.AddModelError("", "Invalid login attempt!");
            return View(model);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return Redirect("~/Index");
        }
    }
}
