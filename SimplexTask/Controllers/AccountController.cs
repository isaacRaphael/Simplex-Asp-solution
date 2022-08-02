using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplexTask.Contracts;
using SimplexTask.Models.Entities;
using SimplexTask.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, 
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "user does not exist");
                return View(model);

            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "incorrect email or password");
                return View(model);
            }

            var roleCheck = await _userManager.IsInRoleAsync(user, "Admin");

            if(!roleCheck)
                return RedirectToAction("UserDashboard", "Home", new { email = model.Email });

            return RedirectToAction("AdminDash", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public  IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var checkuser = await _userManager.FindByEmailAsync(model.Email);
            if (checkuser is not null)
            {
                ModelState.AddModelError("", "email already in use");
                return View(model);
            }
            
            var user = new User
            {
                Fullname = $"{model.Firstname} {model.Lastname}",
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", string.Join("\n", result.Errors));
                return View(model);
            }
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("UserDashboard", "Home", new { email = user.Email });
        }
    }
}
