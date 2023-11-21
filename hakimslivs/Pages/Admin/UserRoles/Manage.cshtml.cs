using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using hakimslivs.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace hakimslivs.Pages.Admin.UserRoles
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ManageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public List<ManageUserRolesViewModel> ManageUserRolesViewModels { get; set; }
        [BindProperty]
        public ManageUserRolesViewModel ManageUserRolesViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"User with Id = {userId} cannot be found");
                return NotFound();
            }

            List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            ManageUserRolesViewModels = model;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Page();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return Page();
            }

            result = await _userManager.AddToRolesAsync(user, ManageUserRolesViewModels.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}