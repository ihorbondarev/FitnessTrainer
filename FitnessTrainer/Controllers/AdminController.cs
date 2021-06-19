using System.Threading.Tasks;
using FitnessTrainer.DomainEntities.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FitnessTrainer.ViewModels;
using System;
using System.Security.Claims;
using FitnessTrainer.DataAccess.DbContexts;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            return LocalRedirect(returnUrl);
        }

        public IActionResult Index()
        {
            ViewBag.NameOfActivePage = "AdminIndex";
            return View();
        }

        public IActionResult Administrator()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                };

                var result = _userManager.CreateAsync(user, model.Password).GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "MobileUser")).GetAwaiter().GetResult();
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    var userNameExist = _context.ApplicationUsers.Any(x => user.UserName == x.UserName);
                    if(userNameExist)
                    {
                        ModelState.AddModelError("", _localizer["UserNameAlreadyExistsError"]);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if(user == null)
            {
                ModelState.AddModelError("", _localizer["UserNotFoundError"]);
                return View(model);
            } else if(await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                
                if (result.Succeeded)
                {
                    if (user.UserName == "Administrator")
                    {
                        return RedirectToAction("Index", "Admin");
                    } else
                    {
                        return RedirectToAction("Index", "Users");
                    }
                }
            } else
            {
                ModelState.AddModelError("", _localizer["WrongPasswordError"]);
                return View(model);
            }

            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Admin");
        }
    }
}
