using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User_Identity_App.Interfaces;
using User_Identity_App.Models;
using User_Identity_App.ViewModel;

namespace User_Identity_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ISendGridEmail _sendGridEmail;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ISendGridEmail sendGridEmail)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sendGridEmail = sendGridEmail;
        }


        [HttpGet]
        public async Task<IActionResult> login()
        {
            LoginVm loginVm = new LoginVm();

            return View(loginVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVm loginVm)
        {

            if (!ModelState.IsValid) return View(loginVm);

            var user = await _userManager.FindByEmailAsync(loginVm.EmailAddress);
            if (user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, loginVm.Password);
                if (password)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVm.Password, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    if (result.IsLockedOut)
                        return View("LockOut");
                    TempData["Error"] = "Wrong Credentials. please try again!";
                    return View(loginVm);
                }

            }
            TempData["Error"] = "Wrong Credentials. please try again!";
            return View(loginVm);
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVm model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    return View("ForgetPasswordConfirmation");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Account", new { user = user.Id, code = code }, protocol: Request.Scheme);

                await _sendGridEmail.SendEmailAsync(model.EmailAddress, "Reset Emaill Address Confrimation", "Please reset email by going to this"
                    + "<a href=\"" + callbackurl + "\">link</a>");
                return RedirectToAction("ForgetPasswordConfirmation");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm resetPasswordVm)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordVm);

            var user = await _userManager.FindByEmailAsync(resetPasswordVm.EmailAddress);
            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return View(resetPasswordVm);
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordVm.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            return View(resetPasswordVm);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordConfirmation()
        {
            return View();
        }



        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            RegisterVm registerVm = new RegisterVm();
            registerVm.ReturnUrl = returnUrl;
            return View(registerVm);
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterVm registerVm, string? returnUrl = null)
        {
            registerVm.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
                return View(registerVm);
            var user = await _userManager.FindByEmailAsync(registerVm.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This is alreaady in used!";
                return View(registerVm);
            }

            var newUser = new AppUser
            {
                Email = registerVm.EmailAddress,
                UserName = registerVm.Username,
            };

            var reponse = await _userManager.CreateAsync(newUser, registerVm.Password);
            if (reponse.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            return View(registerVm);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
