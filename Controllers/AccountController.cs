using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiMatos.Services;
using MiMatos.ViewModels;

namespace MiMatos.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ISeedUserRoles _seedUserRoles;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ISeedUserRoles seedUserRoles)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _seedUserRoles = seedUserRoles;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null || user.LockoutEnabled)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("Login", "Inválido.");
            return View(loginVM);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser 
                { 
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    NormalizedUserName = registerVM.UserName.ToUpper(),
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                if(user.UserName == "Michele")
                {
                    user.LockoutEnabled = false;
                }
                else
                {
                    user.LockoutEnabled = true;
                }

                var result = _seedUserRoles.SeedUsers(user, registerVM.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("Registro", "Falha ao Registrar o usuário");
                }
            }
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
