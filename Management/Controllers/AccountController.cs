using Application.Services.Email;
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
        private readonly EmailService _emailService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    string callbackUrl = Url.Action("ConfirmEmail", "Account", new { UserId = newUser.Id, token = token }, protocol: Request.Scheme);
                    string body = $"<h1>♦</h1><br/><h3>Please click on the link below to activate your account! ✨</h3> <br/> <h2><a href={callbackUrl}> Account Verification ✔</a></h2>";

                    await _emailService.Execute(newUser.Email, body, "News site: User account activation👋");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("DisplayEmail");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        return LocalRedirect("~");
                    }
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(model);
        }

        public IActionResult DisplayEmail()
        {
            return View();
        }

        public IActionResult ConfirmEmail(string UserId, string Token)
        {
            if (UserId == null || Token == null)
            {
                return BadRequest();
            }
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            var result = _userManager.ConfirmEmailAsync(user, Token).Result;
            if (result.Succeeded)
            {
                _signInManager.SignInAsync(user, true).Wait();
                return LocalRedirect("~/Index");
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult Login(string returnUtl = "/")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUtl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.IsPersistent, true);

                if (result.Succeeded)
                {
                    return LocalRedirect(model.ReturnUrl);
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "User account locked out!");
                    return View(model);
                }
                //if (result.RequiresTwoFactor)
                //{
                //
                //}
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt!");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return LocalRedirect("~/Index");
        }
    }
}
