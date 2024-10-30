using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Helpers;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public AccountController(IConfiguration configuration, SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public IActionResult Post(string email, string password)
        {
            _logger.Info("Post method called with email: {Email}", email);

            // Attempt to sign in the user with the provided email and password
            var findUser = signInManager.PasswordSignInAsync(email, password, true, true).Result;

            if (findUser.Succeeded)
            {
                _logger.Info("User signed in successfully for email: {Email}", email);

                var user = userManager.FindByEmailAsync(email).Result;

                // Create a JWT token for the authenticated user
                var data = CreateToken(user);

                return Ok(new LoginResultDto
                {
                    IsSuccess = true,
                    Data = data
                });
            }

            _logger.Warn("Failed sign-in attempt for email: {Email}", email);

            return Unauthorized();
        }

        private string CreateToken(User user)
        {
            SecurityHelper securityHelper = new SecurityHelper();

            // Define the claims to be included in the JWT token
            var claims = new List<Claim>
                {
                    new Claim ( "UserId", user.Id),
                    new Claim ( "Name", user?.FullName??""),
                };

            // Retrieve the JWT configuration key from the application settings
            string Key = configuration["JWtConfig:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

            // Create signing credentials using the secret key and HMAC SHA256 algorithm
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Set the expiration time for the token
            var tokenexp = DateTime.Now.AddMinutes(int.Parse(configuration["JWtConfig:expires"]));

            // Create the JWT token with the specified claims and settings
            var token = new JwtSecurityToken(
                issuer: configuration["JWtConfig:issuer"],
                audience: configuration["JWtConfig:audience"],
                expires: tokenexp,
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: credentials
                );

            // Generate the token string
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Save the token in the database or another storage
            var data = SaveTokens(new UserTokenDto
            {
                Id = user.Id,
                LoginProvider = "JWT",
                Name = "AccessToken",
                Value = securityHelper.Getsha256Hash(jwtToken),
                User = user,
            });

            // Return the generated token if saved successfully
            if (data)
            {
                return jwtToken;
            }

            return null;
        }

        private bool SaveTokens(UserTokenDto dto)
        {
            // Attempt to save the authentication token for the user
            var result = userManager.SetAuthenticationTokenAsync(dto.User, dto.LoginProvider, dto.Name, dto.Value).Result;

            // Return true if the token was saved successfully
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
