using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace hakimslivs.Pages.Admin.RoleManager
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class IndexModel : PageModel
    {
        RoleManager<IdentityRole> _roleManager;

        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }
        public List<IdentityRole> Roles { get; set; }

        public IdentityRole Role { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _roleManager.Roles.ToListAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IdentityRole role)
        {
            if (role.Name != null)
            {
                Role = new IdentityRole
                {
                    Name = role.Name.Trim()
                };
                await _roleManager.CreateAsync(Role);
            }
            return RedirectToPage();
        }
    }
}