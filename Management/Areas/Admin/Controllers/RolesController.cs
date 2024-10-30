using Domain.Roles;
using Domain.Users;
using Management.Areas.Admin.Models.Dtos.Roles;
using Management.Areas.Admin.Models.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Data;

namespace Management.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            _logger.Info("Index action in Roles Controller called."); // Log when the Index action is invoked

            // Retrieve all roles from the role manager and project them into a list of RoleListDto objects
            var roles = _roleManager.Roles
                .Select(p => new RoleListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .ToList();

            _logger.Info($"Retrieved {roles.Count} roles."); // Log the number of roles retrieved
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddNewRoleDto newRole)
        {
            // Create a new Role object using the data from the newRole DTO
            Role role = new Role()
            {
                Description = newRole.Description,
                Name = newRole.Name,
            };

            var result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                // Log successful creation of the role
                _logger.Info("Role '{RoleName}' created successfully.", role.Name);

                return RedirectToAction("Index", "Roles", new { area = "Admin" });
            };
            ViewBag.Errors = result.Errors.ToList();

            return View(newRole);
        }

        public IActionResult UserInRole(string Name)
        {
            // Get the list of users associated with the specified role name asynchronously
            var usersInRole = _userManager.GetUsersInRoleAsync(Name).Result;

            if (usersInRole == null || !usersInRole.Any())
            {
                _logger.Warn($"No users found for role name: {Name}");
            }
            else
            {
                _logger.Info($"Found {usersInRole.Count()} users for role name: {Name}");
            }

            return View(usersInRole.Select(p => new UserListDto
            {
                FullName = p.FullName,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Id = p.Id,
            }));
        }
    }
}
