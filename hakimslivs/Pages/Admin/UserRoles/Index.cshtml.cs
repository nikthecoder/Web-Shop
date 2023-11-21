using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakimslivs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace hakimslivs.Pages.Admin.UserRoles
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderator, Basic")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public List<UserRolesViewModel> UserRolesViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
            {
                var users = await _userManager.Users.ToListAsync();
                UserRolesViewModel = new List<UserRolesViewModel>();
                foreach (ApplicationUser user in users)
                {
                    var thisViewModel = new UserRolesViewModel();
                    thisViewModel.UserId = user.Id;
                    thisViewModel.Email = user.Email;
                    thisViewModel.FirstName = user.FirstName;
                    thisViewModel.LastName = user.LastName;
                    thisViewModel.Roles = await GetUserRoles(user);
                    UserRolesViewModel.Add(thisViewModel);
                }

            }
            else
            {
                var LoggedInUserID = _userManager.GetUserId(HttpContext.User);
                var users = await _userManager.Users.Where(u => u.Id == LoggedInUserID).ToListAsync();
                UserRolesViewModel = new List<UserRolesViewModel>();
                foreach (ApplicationUser user in users)
                {
                    var thisViewModel = new UserRolesViewModel();
                    thisViewModel.UserId = user.Id;
                    thisViewModel.Email = user.Email;
                    thisViewModel.FirstName = user.FirstName;
                    thisViewModel.LastName = user.LastName;
                    thisViewModel.Roles = await GetUserRoles(user);
                    UserRolesViewModel.Add(thisViewModel);
                }
            }
            return Page();
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}