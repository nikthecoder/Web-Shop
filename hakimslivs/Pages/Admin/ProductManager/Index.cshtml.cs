using System;
using hakimslivs.Data;
using hakimslivs.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace hakimslivs.Pages.Admin.ProductManager
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }
        public List<IdentityRole> Roles { get; set; }
        public IList<Item> Item { get;set; }
        public IList<Category> Categories { get; set; }
        public string CurrentCategory { get; set; }

        public async Task OnGetAsync(string currentCategory)
        {
            Roles = await _roleManager.Roles.ToListAsync();
            Categories = await _context.Categories.ToListAsync();
            if (!String.IsNullOrEmpty(currentCategory))
            {
                Item = await _context.Items.Include(i => i.Category).Where(i => i.Category.Name == currentCategory).ToListAsync();
            }
            else
            {
                Item = await _context.Items.Include(i => i.Category).ToListAsync();
            }
        }
    }
}
