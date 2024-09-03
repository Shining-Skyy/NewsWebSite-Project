using Application.Services.Email;
using Application.Services.Google;
using Application.Services.Sms;
using Domain.Users;
using Management.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using NuGet.Common;
using System.Security.Claims;
using System.Security.Policy;

namespace Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        private readonly SmsService _smsService;
        private readonly GoogleRecaptcha _googleRecaptcha;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            EmailService emailService, SmsService smsService, GoogleRecaptcha googleRecaptcha)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _smsService = smsService;
            _googleRecaptcha = googleRecaptcha;
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
                if (!await VerifyGoogleRecaptcha())
                    return View(model);

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
                    await SendEmailConfirmationToken(newUser);
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
                await _signInManager.SignInAsync(user, true);
                return LocalRedirect("~/Index");
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        public async void EmailConfirmationAfterLogin()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await SendEmailConfirmationToken(user);
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
                if (!await VerifyGoogleRecaptcha())
                    return View(model);

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.IsPersistent, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return LocalRedirect(model.ReturnUrl);
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "User account locked out!");
                    return View(model);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("TowFactorLogin", new { model.Email, model.IsPersistent });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt!");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult ExternalLogin(string ReturnUrl)
        {
            string url = Url.Action(nameof(CallBack), "Account", new
            {
                ReturnUrl
            });

            var propertis = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);

            return new ChallengeResult("Google", propertis);
        }

        public async Task<IActionResult> CallBack(string ReturnUrl)
        {
            try
            {
                var loginInfo = await _signInManager.GetExternalLoginInfoAsync();

                string email = loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value ?? null;
                if (email == null)
                {
                    return BadRequest();
                }
                string FirstName = loginInfo.Principal.FindFirst(ClaimTypes.GivenName)?.Value ?? null;
                string LastName = loginInfo.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? null;
                string FullName = FirstName + LastName;

                var signin = await _signInManager.ExternalLoginSignInAsync("Google", loginInfo.ProviderKey, false, true);
                if (signin.Succeeded)
                {
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect("Login");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else if (signin.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Your login failed!");
                    return Redirect("/");
                }
                else if (signin.IsLockedOut)
                {
                    ModelState.AddModelError("", "User account locked out!");
                    return Redirect("/");
                }
                else if (signin.RequiresTwoFactor)
                {
                    return RedirectToAction("TowFactorLogin");
                }
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    User newUser = new User()
                    {
                        UserName = email,
                        Email = email,
                        FullName = FullName,
                        EmailConfirmed = true,
                    };
                    var resultAdduser = _userManager.CreateAsync(newUser).Result;
                    user = newUser;
                }
                var resultAddlogin = await _userManager.AddLoginAsync(user, loginInfo);
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("/");
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
                if (!await VerifyGoogleRecaptcha())
                    return View(model);

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "The email entered is not valid!");
                    return View();
                }
                else
                {
                    await SendPasswordResetToken(user);
                    return View();
                }
            }
            return View(model);
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
                if (!await VerifyGoogleRecaptcha())
                    return View(model);

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
                    return LocalRedirect("~/Index");
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

        [Authorize]
        public async Task<IActionResult> PhoneNumberConfirmationAfterLogin()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            try
            {
                //await _smsService.Send(user.PhoneNumber, token);
                return RedirectToAction("VerifyPhoneNumber");
            }
            catch (Exception)
            {
                return LocalRedirect("~/Index");
            }
        }

        [Authorize]
        public IActionResult VerifyPhoneNumber()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await VerifyGoogleRecaptcha())
                    return View(model);

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Token, user.PhoneNumber);
                if (result)
                {
                    user.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    return LocalRedirect("~/Index");
                }
                else
                {
                    ModelState.AddModelError("", "User account locked out!");
                    return View();
                }
            }
            return View(model);
        }

        public async Task<IActionResult> TowFactorLogin(string email, bool isPersistent)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest();
            }

            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            TwoFactorLoginViewModel model = new TwoFactorLoginViewModel();
            if (providers.Contains("Email"))
            {
                await SendTwoFactorToken(user);
                model.Provider = "Email";
                TempData["Provider"] = model.Provider;
                model.IsPersistent = isPersistent;
                ViewBag.message = "Your two-factor login code has been sent to your email";
            }
            else if (providers.Contains("Phone"))
            {
                try
                {
                    string smsCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");
                    await _smsService.Send(user.PhoneNumber, smsCode);
                    model.Provider = "Phone";
                    TempData["Provider"] = model.Provider;
                    model.IsPersistent = isPersistent;
                    ViewBag.message = "Your two-factor login code has been sent to your mobile";
                }
                catch (Exception)
                {
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TowFactorLogin(TwoFactorLoginViewModel model)
        {

            if (!await VerifyGoogleRecaptcha())
                return View(model);

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                ModelState.AddModelError("", "The email entered is not valid!");
                return View();
            }
            var result = await _signInManager.TwoFactorSignInAsync(TempData["Provider"].ToString(), model.Code, model.IsPersistent, true);
            if (result.Succeeded)
            {
                return LocalRedirect("~/Index");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "User account locked out!");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "The entered Code is not correct");
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> TwoFactorEnabled()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.EmailConfirmed || user.PhoneNumberConfirmed)
            {
                var result = await _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled);
                return LocalRedirect("~/Index");
            }
            else
            {
                ModelState.AddModelError("", "You must confirm your email or mobile number");
                return LocalRedirect("~/Index");
            }
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("~/Index");
        }

        private async Task SendEmailConfirmationToken(User newUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            string callbackUrl = Url.Action("ConfirmEmail", "Account", new { UserId = newUser.Id, token = token }, protocol: Request.Scheme);
            string body = $"<h1>♦</h1><br/><h3>Please click on the link below to activate your account! ✨</h3><br/><h2><a href={callbackUrl}> Account Verification ✔</a></h2>";
            try
            {
                await _emailService.Execute(newUser.Email, body, "News site: User account activation👋");
                ViewBag.message = "Email confirmation has been sent to you";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "If it was not sent, it is not a problem, your account has been created!");
            }
        }
        private async Task SendPasswordResetToken(User? user)
        {
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
        }
        private async Task SendTwoFactorToken(User? user)
        {
            string emailCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            string body = $"<h1>♦</h1><br/><h3>Your two-step login is done using the code below! ✨</h3><br/><h2>Two Factor Code:{emailCode} ✔</a></h2>";
            try
            {
                await _emailService.Execute(user.Email, body, "News site: User account activation👋");
                ViewBag.message = "Email confirmation has been sent to you";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "If it was not sent, it is not a problem, your account has been created!");
            }
        }
        private async Task<bool> VerifyGoogleRecaptcha()
        {
            string googleResponse = HttpContext.Request.Form["g-Recaptcha-Response"];
            if (await _googleRecaptcha.Verify(googleResponse) == false)
            {
                ModelState.AddModelError("", "Confirm you are not a robot!");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
