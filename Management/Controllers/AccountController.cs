using Application.Services.Email;
using Application.Services.Google;
using Application.Services.Sms;
using Domain.Users;
using Humanizer;
using Management.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Numerics;
using System.Security.Claims;

namespace Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        private readonly SmsService _smsService;
        private readonly GoogleRecaptcha _googleRecaptcha;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
            // Checks if the model state is valid (i.e., all required fields are filled correctly)
            if (ModelState.IsValid)
            {
                if (!await VerifyGoogleRecaptcha()) // Verifies the Google reCAPTCHA to prevent bots
                {
                    _logger.Warn("Google reCAPTCHA verification failed.");
                    return View(model);
                }

                User newUser = new User()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };

                // Creates a new user with the provided password
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    _logger.Info("User created successfully. Sending email confirmation token.");

                    // Sends an email confirmation token to the new user
                    await SendEmailConfirmationToken(newUser);
                    return View();
                }

                // If there are errors during user creation, add them to the model state
                foreach (var item in result.Errors)
                {
                    _logger.Error($"Error creating user: {item.Code} - {item.Description}");
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string UserId, string Token)
        {
            if (UserId == null || Token == null)
            {
                _logger.Warn("UserId or Token is null.");
                return BadRequest();
            }

            // Finds the user by UserId
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                _logger.Warn("User not found for UserId: {UserId}", UserId);
                return BadRequest();
            }

            // Confirms the user's email with the provided token
            var result = await _userManager.ConfirmEmailAsync(user, Token);
            if (result.Succeeded)
            {
                _logger.Info("Email confirmed successfully for UserId: {UserId}", UserId);

                // Signs in the user after email confirmation
                await _signInManager.SignInAsync(user, true);
                return LocalRedirect("~/Index");
            }
            else
            {
                _logger.Warn("Email confirmation failed for UserId: {UserId}", UserId);
                return BadRequest();
            }
        }

        [Authorize]
        public async void EmailConfirmationAfterLogin()
        {
            // Retrieves the currently logged-in user
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                _logger.Info($"User found: {user.UserName}");

                // Sends an email confirmation token to the user
                await SendEmailConfirmationToken(user);
                _logger.Info($"Email confirmation token sent to user: {user.UserName}");
            }

            else
            {
                _logger.Warn("No user found for the current identity.");
            }
        }

        public IActionResult Login(string returnUtl = "/")
        {
            // Returns the login view with the return URL
            return View(new LoginViewModel
            {
                ReturnUrl = returnUtl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Checks if the model state is valid
            if (ModelState.IsValid)
            {
                if (!await VerifyGoogleRecaptcha())
                {
                    _logger.Warn("Google reCAPTCHA verification failed for user: {Email}", model.Email);
                    return View(model);
                }

                // Attempts to sign in the user
                var result = await _signInManager
                    .PasswordSignInAsync(model.Email, model.Password, model.IsPersistent, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.Info("User {Email} signed in successfully.", model.Email);

                    // Redirects to the return URL if sign-in is successful
                    return LocalRedirect(model.ReturnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.Warn("User account locked out: {Email}", model.Email);

                    // Adds an error if the account is locked
                    ModelState.AddModelError("", "User account locked out!");
                    return View(model);
                }
                if (result.RequiresTwoFactor)
                {
                    _logger.Info("Two-factor authentication required for user: {Email}", model.Email);

                    // Redirects to two-factor login if required
                    return RedirectToAction("TowFactorLogin", new { model.Email, model.IsPersistent });
                }
                else
                {
                    _logger.Warn("Invalid login attempt for user: {Email}", model.Email);

                    // Adds an error for invalid login attempts
                    ModelState.AddModelError("", "Invalid login attempt!");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult ExternalLogin(string ReturnUrl)
        {
            // Generates the callback URL for external login
            string url = Url.Action(nameof(CallBack), "Account", new { ReturnUrl });
            _logger.Debug("Callback URL generated: {CallbackUrl}", url);

            // Configures properties for Google authentication
            var propertis = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);
            _logger.Debug("External authentication properties configured for Google.");

            // Initiates the external login challenge
            return new ChallengeResult("Google", propertis);
        }

        public async Task<IActionResult> CallBack(string ReturnUrl)
        {
            try
            {
                // Retrieves external login information
                var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (loginInfo == null)
                {
                    _logger.Warn("No external login information found.");
                    return BadRequest();
                }

                // Gets the email from the 
                string email = loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value ?? null;
                if (email == null)
                {
                    _logger.Warn("Email not found in external login information.");
                    return BadRequest();
                }
                // Gets the first name
                string FirstName = loginInfo.Principal.FindFirst(ClaimTypes.GivenName)?.Value ?? null;

                // Gets the last name
                string LastName = loginInfo.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? null;

                // Combines first and last name
                string FullName = FirstName + LastName;

                // Attempts to sign in with external login
                var signin = await _signInManager.ExternalLoginSignInAsync("Google", loginInfo.ProviderKey, false, true);

                if (signin.Succeeded)
                {
                    _logger.Info($"User {email} signed in successfully.");

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect("Login");
                    }

                    return RedirectToAction("Index", "Home");
                }
                else if (signin.IsNotAllowed)
                {
                    _logger.Warn($"Login failed for user {email}: Not allowed.");

                    ModelState.AddModelError("", "Your login failed!");
                    return Redirect("/");
                }
                else if (signin.IsLockedOut)
                {
                    _logger.Warn($"User account {email} is locked out.");

                    ModelState.AddModelError("", "User account locked out!");
                    return Redirect("/");
                }
                else if (signin.RequiresTwoFactor)
                {
                    _logger.Info($"User {email} requires two-factor authentication.");

                    return RedirectToAction("TowFactorLogin");
                }

                // Finds the user by email
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.Info($"Creating new user with email {email}.");
                    User newUser = new User()
                    {
                        UserName = email,
                        Email = email,
                        FullName = FullName,
                        EmailConfirmed = true,
                    };
                    // Creates a new user
                    var resultAdduser = _userManager.CreateAsync(newUser).Result;
                    if (resultAdduser.Succeeded)
                    {
                        user = newUser;
                        _logger.Info($"New user {email} created successfully.");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                // Adds the external login to the user
                var resultAddlogin = await _userManager.AddLoginAsync(user, loginInfo);
                if (!resultAddlogin.Succeeded)
                {
                    _logger.Error($"Failed to add external login for user {email}");
                    return BadRequest();
                }


                // Signs in the user
                await _signInManager.SignInAsync(user, false);
                _logger.Info($"User {email} signed in after adding external login.");
                return RedirectToAction("/");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred during the callback process: {ex.Message}");
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
            // Check if the model state is valid (i.e., all required fields are filled correctly)
            if (ModelState.IsValid)
            {
                // Verify the Google reCAPTCHA to prevent spam submissions
                if (!await VerifyGoogleRecaptcha())
                {
                    _logger.Warn("Google reCAPTCHA verification failed for email: {Email}", model.Email);
                    return View(model);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.Warn("No user found with the email: {Email}", model.Email);
                    ModelState.AddModelError("", "The email entered is not valid!");
                    return View();
                }
                else
                {
                    // Log that a password reset token is being sent
                    _logger.Info("Sending password reset token to user with email: {Email}", model.Email);

                    // Send a password reset token to the user
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
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Verify the Google reCAPTCHA again
                if (!await VerifyGoogleRecaptcha())
                {
                    _logger.Warn("Google reCAPTCHA verification failed.");
                    return View(model);
                }
                if (model.UserId == null || model.Token == null)
                {
                    _logger.Error("UserId or Token is null.");
                    return BadRequest();
                }

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    _logger.Warn("User not found for UserId: {UserId}", model.UserId);
                    return BadRequest();
                }

                // Attempt to reset the user's password using the provided token and new password
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    _logger.Info("Password reset successful for UserId: {UserId}", model.UserId);
                    return LocalRedirect("~/Index");
                }

                // If there are errors during the password reset, add them to the model state
                foreach (var item in result.Errors)
                {
                    _logger.Error("Error resetting password: {Code} - {Description}", item.Code, item.Description);
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
            if (user == null)
            {
                _logger.Warn("User not found for the given identity name: {UserName}", User.Identity.Name);
                return LocalRedirect("~/Index");
            }

            // Generate a token for changing the user's phone number
            string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            _logger.Info("Token generated for user: {UserName}", user.UserName);

            try
            {
                // Uncomment the line below to send the token via SMS (currently commented out)
                await _smsService.Send(user.PhoneNumber, token);

                _logger.Info("Token sent via SMS to phone number: {PhoneNumber}", user.PhoneNumber);
                return RedirectToAction("VerifyPhoneNumber");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while sending SMS to phone number: {PhoneNumber}", user.PhoneNumber);

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
            // Checks if the model state is valid (i.e., all required fields are filled correctly).
            if (ModelState.IsValid)
            {
                // Verify the Google reCAPTCHA again
                if (!await VerifyGoogleRecaptcha())
                {
                    _logger.Warn("Google reCAPTCHA verification failed for user: {UserName}", User.Identity.Name);
                    return View(model);
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                // Verifies the phone number change token provided by the user.
                var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Token, user.PhoneNumber);
                if (result)
                {
                    user.PhoneNumberConfirmed = true;

                    // Updates the user in the database.
                    await _userManager.UpdateAsync(user);

                    _logger.Info("Phone number verified and updated for user: {UserName}", user.UserName);

                    return LocalRedirect("~/Index");
                }
                else
                {
                    _logger.Warn("Phone number verification failed for user: {UserName}. User account may be locked out.", user.UserName);

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
                _logger.Warn($"User not found for email: {email}");
                return BadRequest();
            }

            // Retrieves valid two-factor
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            TwoFactorLoginViewModel model = new TwoFactorLoginViewModel();

            // Checks if Email is a valid two-factor provider.
            if (providers.Contains("Email"))
            {
                _logger.Info("Email is a valid two-factor provider. Sending token to email.");

                // Sends a two-factor token to the user's email.
                await SendTwoFactorToken(user);

                model.Provider = "Email";
                TempData["Provider"] = model.Provider;
                model.IsPersistent = isPersistent;

                ViewBag.message = "Your two-factor login code has been sent to your email";
            }
            // Checks if Phone is a valid two-factor provider.
            else if (providers.Contains("Phone"))
            {
                try
                {
                    _logger.Info("Phone is a valid two-factor provider. Generating token for phone.");

                    // Generates a two-factor token for phone.
                    string smsCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");

                    // Sends the token via SMS.
                    await _smsService.Send(user.PhoneNumber, smsCode);

                    model.Provider = "Phone";
                    TempData["Provider"] = model.Provider;
                    model.IsPersistent = isPersistent;

                    ViewBag.message = "Your two-factor login code has been sent to your mobile";
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while sending SMS for two-factor login.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TowFactorLogin(TwoFactorLoginViewModel model)
        {
            // Verifies the Google reCAPTCHA to prevent bots.
            if (!await VerifyGoogleRecaptcha())
            {
                _logger.Warn("Google reCAPTCHA verification failed.");
                return View(model);
            }
            // Retrieves the user for two-factor authentication.
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                _logger.Error("Two-factor authentication user not found.");

                ModelState.AddModelError("", "The email entered is not valid!");
                return View();
            }

            // Attempts to sign in the user using the two-factor authentication code.
            var result = await _signInManager
                .TwoFactorSignInAsync(TempData["Provider"]
                .ToString(), model.Code, model.IsPersistent, true);

            if (result.Succeeded)
            {
                _logger.Info($"User {user.Email} successfully signed in.");

                return LocalRedirect("~/Index");
            }
            else if (result.IsLockedOut)
            {
                _logger.Warn($"User {user.Email} account is locked out.");

                ModelState.AddModelError("", "User account locked out!");
                return View();
            }
            else
            {
                _logger.Warn($"Failed sign-in attempt for user {user.Email}. Incorrect code entered.");

                ModelState.AddModelError("", "The entered Code is not correct");
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> TwoFactorEnabled()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                _logger.Warn("User not found: " + User.Identity.Name);
                return LocalRedirect("~/Index");
            }

            // Checks if the user's email or phone number is confirmed.
            if (user.EmailConfirmed || user.PhoneNumberConfirmed)
            {
                // Toggles the two-factor authentication setting.
                var result = await _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled);
                if (result.Succeeded)
                {
                    _logger.Info("Two-factor authentication setting toggled successfully.");
                }
                else
                {
                    _logger.Error("Failed to toggle two-factor authentication setting for user: " + user.UserName);
                }

                return LocalRedirect("~/Index");
            }
            else
            {
                _logger.Warn("User must confirm their email or mobile number before enabling two-factor authentication.");

                ModelState.AddModelError("", "You must confirm your email or mobile number");
                return LocalRedirect("~/Index");
            }
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            _logger.Info("User is attempting to log out."); // Log the logout attempt

            // Signs the user out of the application.
            await _signInManager.SignOutAsync();

            _logger.Info("User has successfully logged out."); // Log successful logout

            return LocalRedirect("~/Index");
        }

        private async Task SendEmailConfirmationToken(User newUser)
        {
            // Generate a unique email confirmation token for the new user
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            // Create a callback URL that the user will click to confirm their email
            string callbackUrl = Url
                .Action("ConfirmEmail", "Account", new { UserId = newUser.Id, token = token }, protocol: Request.Scheme);

            // Construct the email body with a link for account verification
            string body = $"<h1>♦</h1><br/><h3>Please click on the link below to activate your account! ✨</h3><br/><h2><a href={callbackUrl}> Account Verification ✔</a></h2>";

            try
            {
                // Send the email using the email service
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
            // Generate a unique password reset token for the user
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create a callback URL for resetting the password
            string callbackUrl = Url
                .Action("ResetPassword", "Account", new { UserId = user.Id, Token = token }, protocol: Request.Scheme);

            // Construct the email body with a link for password reset
            string body = $"<h1>♦</h1><br/><h3>Click the link below to reset your password! ✨</h3> <br/> <h2><a href={callbackUrl}> Set password ✔</a></h2>";
            try
            {
                // Send the password reset email
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
            // Generate a two-factor authentication token for the user
            string emailCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            // Construct the email body with the two-factor code
            string body = $"<h1>♦</h1><br/><h3>Your two-step login is done using the code below! ✨</h3><br/><h2>Two Factor Code:{emailCode} ✔</a></h2>";

            try
            {
                // Send the two-factor authentication email
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
            // Retrieve the Google reCAPTCHA response from the form submission
            string googleResponse = HttpContext.Request.Form["g-Recaptcha-Response"];

            // Verify the reCAPTCHA response to ensure the user is not a robot
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
