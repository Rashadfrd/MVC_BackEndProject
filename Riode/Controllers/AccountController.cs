using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode.Models;
using Riode.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Riode.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }

        public const string templatePath = @"EmailTemplate/{0}.html";
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult MyAccount()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginVM user)
        {
            if (!ModelState.IsValid) return View();
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if (existUser is null)
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(existUser, user.Password, user.RememberMe, true);
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
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
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
            await _signInManager.SignInAsync(user1, true);
            SendEmail(user1.Email, "Malades",user1.Name);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            var appUser = await _userManager.GetUserAsync(User);
            string userId = appUser.Id;
            var user = await _userManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                var check = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.ConfirmNewPassword);
                if (check.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var err in check.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View();
        }

        private async void SendEmail(string email,string subject,string name)
        {
            string myEmail = "rashadnf@code.edu.az";
            string pass = "qxofvewoevlvrhdu";

            var from = new MailAddress(myEmail, "Riode Support");
            var to = new MailAddress(email);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(myEmail, pass)
            };
            using (var message = new MailMessage(from, to) { Subject = subject, Body = await UpdateNamePlaceHolder(GetEmailBody("EmailTemplate"),name),IsBodyHtml = true })
            {
                smtp.Send(message);
            }
        }
         private string GetEmailBody(string template)
         {
            var body = System.IO.File.ReadAllText(string.Format(templatePath,template));
            return body;
         }

        private async Task<string> UpdateNamePlaceHolder(string text, string name)
        {
            string placeHolder = "{{Username}}";
            if (text.Contains(placeHolder))
            {
                text = text.Replace(placeHolder, name);
            }
            return text;
        }

    }
}
