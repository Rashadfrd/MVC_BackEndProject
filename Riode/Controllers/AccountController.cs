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
        private readonly IConfiguration _configuration;

        public const string templatePath = @"EmailTemplate/{0}.html";
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
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
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user1);
            if (!string.IsNullOrEmpty(token))
            {
               SendEmailConfirmationEmail(user1,"Confirm your email address to get started on Riode",token);
               ViewBag.IsEmailSent = true;
            }
            return View();
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

        #region BasicEmailSend
        //private async void SendEmail(string email,string subject,string name)
        //{
        //    string myEmail = "rashadnf@code.edu.az";
        //    string pass = "qxofvewoevlvrhdu";

        //    var from = new MailAddress(myEmail, "Riode Support");
        //    var to = new MailAddress(email);

        //    SmtpClient smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        Credentials = new NetworkCredential(myEmail, pass)
        //    };
        //    using (var message = new MailMessage(from, to) { Subject = subject, Body = await UpdateNamePlaceHolder(GetEmailBody("EmailTemplate"),name),IsBodyHtml = true })
        //    {
        //        smtp.Send(message);
        //    }
        //}
        #endregion

        private async void SendEmailConfirmationEmail(AppUser user, string subject,string token)
        {
            string myEmail = "rashadnf@code.edu.az";
            string password = "qxofvewoevlvrhdu";

            var from = new MailAddress(myEmail, "Riode Support");
            var to = new MailAddress(user.Email);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(myEmail, password)
            };
            using (var message = new MailMessage(from, to) { Subject = subject, Body = await UpdateNamePlaceHolder(GetEmailBody("EmailConfirm"), user,token), IsBodyHtml = true })
            {
                smtp.Send(message);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token =  token.Replace(' ', '+');
                var check = await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
                if (check.Succeeded)
                {
                    ViewBag.IsVerified = true;
                }
            }
            return View();
        }
        private string GetEmailBody(string template)
         {
            var body = System.IO.File.ReadAllText(string.Format(templatePath,template));
            return body;
         }

        private async Task<string> UpdateNamePlaceHolder(string text, AppUser user, string token)
        {
            //string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            //string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            string appDomain = "http://localhost:4272/";
            string confirmationLink = "confirm-email?uid={0}&token={1}";

            string placeHolderLink = "{{Link}}";
            string placeHolder = "{{Username}}";
            if (text.Contains(placeHolder))
            {
                text = text.Replace(placeHolder, user.Name);
            }
            if (text.Contains(placeHolderLink))
            {
                text = text.Replace(placeHolderLink, string.Format(appDomain+confirmationLink,user.Id,token));
            }
            return text;
        }

    }
}
