using hakimslivs.Data;
using hakimslivs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hakimslivs.Pages.Checkout
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderator, Basic")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ApplicationUser IdentityUser { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _roleManager.Roles.ToListAsync();
            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser = await _userManager.FindByIdAsync(UserID);

            return Page();
        }
    }
}