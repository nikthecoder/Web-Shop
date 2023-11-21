using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using hakimslivs.Data;
using hakimslivs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace hakimslivs.Pages.Admin.ProductManager
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }
        public List<IdentityRole> Roles { get; set; }

        [BindProperty]
        public Item Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Roles = await _roleManager.Roles.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }

            Item = await _context.Items.Include(i => i.Category).FirstOrDefaultAsync(m => m.ID == id);

            if (Item == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Roles = await _roleManager.Roles.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }

            Item = await _context.Items.FindAsync(id);

            if (Item != null)
            {
                _context.Items.Remove(Item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
