using Domain.Roles;
using Domain.Users;
using Management.Areas.Admin.Models.Dtos.Roles;
using Management.Areas.Admin.Models.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Management.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            // Retrieve a list of users and map them to UserListDto objects
            var user = _userManager.Users.Select(p => new UserListDto()
            {
                Id = p.Id,
                FullName = p.FullName,
                Email = p.Email,
                EmailConfirmed = p.EmailConfirmed,
                PhoneNumber = p.PhoneNumber,
                PhoneConfirmed = p.EmailConfirmed,
                TwoFactorEnabled = p.TwoFactorEnabled,
            }).ToList();

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RegisterUserDto register)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Create a new User object with the provided registration details
                User newUser = new User()
                {
                    FullName = register.
                    Email = register.Email,
                    UserName = register.Email,
                };

                var result = _userManager.CreateAsync(newUser, register.Password).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "user", new { area = "admin" });
                }

                // If there are errors, concatenate them into a message
                string message = "";
                foreach (var item in result.Errors.ToList())
                {
                    message += item.Description + Environment.NewLine;
                }
                TempData["Message"] = message;

                return View(register);
            }

            return View(register);
        }

        public IActionResult Edit(string Id)
        {
            // Find the user by ID asynchronously
            var user = _userManager.FindByIdAsync(Id).Result;

            // Create an EditUserDto object to hold the user's current details
            EditUserDto userEdit = new EditUserDto()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
            };

            return View(userEdit);
        }

        [HttpPost]
        public IActionResult Edit(EditUserDto userEdit)
        {
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;

            // Update the user's details with the edited information
            user.FullName = userEdit.FullName;
            user.PhoneNumber = userEdit.PhoneNumber;
            user.Email = userEdit.Email;
            user.UserName = userEdit.Email;

            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }

            // If there are errors, concatenate them into a message
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View(userEdit);
        }

        public IActionResult Delete(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;

            // Create a DeleteUserDto object to hold the user's details for deletion confirmation
            DeleteUserDto userDelete = new DeleteUserDto()
            {
                FullName = user.FullName,
                Id = user.Id,
            };

            return View(userDelete);
        }

        [HttpPost]
        public IActionResult Delete(DeleteUserDto userDelete)
        {
            var user = _userManager.FindByIdAsync(userDelete.Id).Result;

            var result = _userManager.DeleteAsync(user).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });

            }

            // If there are errors, concatenate them into a message
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View(userDelete);
        }

        public IActionResult AddUserRole(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;

            // Retrieve the list of roles and create SelectListItem objects for the view
            var roles = new List<SelectListItem>(
                _roleManager.Roles.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Name,
                }
                ).ToList());

            return View(new AddUserRoleDto
            {
                Id = Id,
                Roles = roles,
                Email = user.Email,
                FullName = user.FullName,
            });
        }

        [HttpPost]
        public IActionResult AddUserRole(AddUserRoleDto newRole)
        {
            // Find the user by ID asynchronously
            var user = _userManager.FindByIdAsync(newRole.Id).Result;
            if (user == null)
            {
                return BadRequest();
            }

            // Attempt to add the user to the specified role asynchronously
            var result = _userManager.AddToRoleAsync(user, newRole.Role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Users", new { area = "admin" });
            }

            // If there are errors, concatenate them into a message
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View(newRole);
        }

        public IActionResult UserRoles(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;

            // Retrieve the roles assigned to the user asynchronously
            var roles = _userManager.GetRolesAsync(user).Result;

            // Store user information in ViewBag for display in the view
            ViewBag.UserInfo = $"Name : {user.FullName} || Email : {user.Email}";

            return View(roles);
        }
    }
}
