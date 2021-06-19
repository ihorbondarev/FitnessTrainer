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
using FitnessTrainer.Services.Interfaces;
using X.PagedList;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "MobileUser")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IUserService _userService;
        private readonly IWorkoutPlanService _workoutService;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IStringLocalizer<SharedResource> localizer,
            IUserService userService,
            IWorkoutPlanService workoutService
            )
        {
            _context = context;
            _localizer = localizer;
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
            _workoutService = workoutService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Guid userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser newuser = await _userManager.GetUserAsync(User);
            UserViewModel model = await _userService.GetUserViewModelById(newuser.Id);
            ViewBag.NameOfActivePage = "UserIndex";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserPlans(string searchString, int? page, int pageSize = 5)
        {
            ViewData["CurrentFilter"] = searchString;

            List<WorkoutPlan> plans = await _workoutService.GetWorkoutPlans(searchString);

            int pageNumber = (page ?? 1);
            ViewBag.workoutPlansList = plans.ToPagedList(pageNumber, pageSize);
            ViewBag.NameOfActivePage = "UserPlans";


            return View(plans);
        }


        public async Task<IActionResult> AddWorkoutPlanToUserPlans(int? id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var result = await _userService.AddWorkoutPlanToUserPlans(user.Id, id);

            if (!result)
            {
                return RedirectToAction("GetSubscription", "Users");
            }

            return RedirectToAction("Index", "Users");
        }

        public async Task<IActionResult> GetSubscription()
        {
            return View();
        }

        public async Task<IActionResult> DeletePlanFromUsersPlans(int? id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var result = await _userService.DeletePlanFromUsersPlans(user.Id, id);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Users");
        }

        public async Task<IActionResult> ApplyPremiumStatus()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var result = await _userService.ApplyPremiumStatusForUser(user.Id);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("SuccessGetPremiumSubscription", "Users");
        }

        public async Task<IActionResult> SuccessGetPremiumSubscription()
        {
            return View();
        }

        public async Task<IActionResult> UserPlanDetails(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            WorkoutPlanViewModel plan = await _workoutService.GetWorkoutPlanViewModelById(id);

            if(plan == null)
            {
                return NotFound();
            }

            List<Exercise> exlist = plan.Exercises;

            ViewBag.Exercises = exlist;

            return View(plan);
        }

        public async Task<IActionResult> EditUserProfile(Guid? userid)
        {
            UserViewModel user = await _userService.GetUserViewModelById(userid);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile([Bind("Id, Age, Weight, Height")] ApplicationUser user)
        {
            if(user.Id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _userService.EditUserProfile(user);
            }

            var model = await _userService.GetUserViewModelById(user.Id);

            return RedirectToAction("Index", "Users");
        }
    }
}
