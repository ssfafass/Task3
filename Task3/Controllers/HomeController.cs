using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using Task3.Areas.Identity.Data;
using Task3.Models;

namespace Task3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            List<UsersViewModel> vm = new();

            var items = _userManager.Users.ToList();

            foreach (var item in items)
            {
                vm.Add(new UsersViewModel
                {
                    Id = item.Id,
                    Email = item.Email,
                    FullName = item.FullName,
                    LoginDate = item.LoginDate,
                    RegistrationDate = item.RegistrationDate,
                    Status = item.Status
                });
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> UnBlock(IEnumerable<UsersViewModel> vm)
        {
            foreach (var item in vm)
            {
                if (item.IsSelected.Selected)
                {
                    var selectUser = await _userManager.FindByIdAsync(item.Id);
                    if (selectUser.Status == Status.Blocked)
                    {
                        selectUser.Status = Status.Offline;
                        var lockoutEndDate = DateTime.UtcNow;
                        await _userManager.SetLockoutEnabledAsync(selectUser, false);
                        await _userManager.SetLockoutEndDateAsync(selectUser, lockoutEndDate);
                    }
                }
            }

            return Redirect("/");
        }

        [HttpPost]
        public async Task<ActionResult> Block(IEnumerable<UsersViewModel> vm)
        {
            foreach (var item in vm)
            {
                if (item.IsSelected.Selected)
                {
                    var selectUser = await _userManager.FindByIdAsync(item.Id);

                    if (selectUser.Status != Status.Blocked)
                    {
                        selectUser.Status = Status.Blocked;
                        var lockoutEndDate = new DateTime(2999, 01, 01);
                        await _userManager.SetLockoutEnabledAsync(selectUser, true);
                        await _userManager.SetLockoutEndDateAsync(selectUser, lockoutEndDate);
                        if (selectUser.UserName.Equals(User?.Identity?.Name))
                        {
                            await _signInManager.SignOutAsync();
                        }
                    }
                }
            }

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(IEnumerable<UsersViewModel> vm)
        {
            foreach (var item in vm)
            {
                if (item.IsSelected.Selected)
                {
                    var selectUser = await _userManager.FindByIdAsync(item.Id);
                    await _userManager.DeleteAsync(selectUser);
                    if (selectUser.UserName.Equals(User?.Identity?.Name))
                    {
                        await _signInManager.SignOutAsync();
                    }
                }
            }

            return Redirect("/");
        }
    }
}