using Application.Services.Email;
using Domain.Users;
using Management.ViewModels.Account;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using NuGet.Common;
using System.Security.Policy;

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
                    try
                    {
                        await _emailService.Execute(newUser.Email, body, "News site: User account activation👋");
                        ViewBag.message = "Email confirmation has been sent to you";
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "If it was not sent, it is not a problem, your account has been created!");
                    }
                    return View();
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string UserId, string Token)
        {
            if (UserId == null || Token == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, Token);
            if (result.Succeeded)
            {
                _signInManager.SignInAsync(user, true);
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

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "The email entered is not valid!");
                    return View();
                }
                if (await _userManager.IsEmailConfirmedAsync(user) == false)
                {
                    ModelState.AddModelError("", "You must confirm your email to change your password!");
                    return View();
                }
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, Token = token }, protocol: Request.Scheme);
                string body = $"<h1>♦</h1><br/><h3>Click the link below to reset your password! ✨</h3> <br/> <h2><a href={callbackUrl}> Set password ✔</a></h2>";
                try
                {
                    await _emailService.Execute(user.Email, body, "News site: forget password👋");
                    ViewBag.message = "Password reset link has been sent to your email";
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Failed to send email, please try again");
                }
                return View();
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult ResetPassword(string UserId, string Token)
        {
            return View(new ResetPasswordViewModel
            {
                Token = Token,
                UserId = UserId,
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserId == null || model.Token == null)
                {
                    return BadRequest();
                }
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return BadRequest();
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return LocalRedirect(nameof(Login));
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return LocalRedirect("~/Index");
        }
    }
}
