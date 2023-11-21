using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using hakimslivs.Data;
using hakimslivs.Models;


namespace hakimslivs.Pages
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CartModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Item> Items { get; set; }


        public async Task OnGetAsync(string currentCategory)
        {

            Items = await _context.Items.Include(i => i.Category).ToListAsync();

        }

    }
}
