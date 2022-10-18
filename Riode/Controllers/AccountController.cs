using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode.Models;
using Riode.ViewModels;

namespace Riode.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginVM user)
        {
            if(!ModelState.IsValid) return View();
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if (existUser is null)
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(existUser, user.Password, user.RememberMe,true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You made too many attemtps. Wait until -> " + existUser.LockoutEnd?.AddHours(4).DateTime.ToString("MM/dd/yyyy HH:mm:ss"));
                    return View();
                }
                ModelState.AddModelError("", "Username or password is wrong");
                return View();
            }
            return RedirectToAction("Index","Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationVM user)
        {
            if (!ModelState.IsValid) return View();
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if (existUser != null)
            {
                ModelState.AddModelError("Email", "This email has already been taken. Please,try too add new one");
                return View();
            }

            AppUser user1 = new AppUser
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.Name
            };
            var result = await _userManager.CreateAsync(user1, user.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.ToString());
                }
                return View();
            }
            await _signInManager.SignInAsync(user1,true);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
