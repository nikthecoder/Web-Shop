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

namespace hakimslivs.Pages.Admin.OrderManager
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DetailsModel : PageModel
    {
        private readonly hakimslivs.Data.ApplicationDbContext _context;

        public DetailsModel(hakimslivs.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }
        public List<ItemQuantity> ItemQuantityList { get; set; }
        public List<Item> Items { get; set; }
        public decimal Total { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.Include(o => o.OrderStatus).Include(o => o.User).FirstOrDefaultAsync(m => m.ID == id);
            ItemQuantityList = await _context.ItemQuantities.Include(iq => iq.Item).Where(iq => iq.OrderID == Order.ID).ToListAsync();
            Items = await _context.Items.ToListAsync();

            Total = 0;

            foreach(var itemQuantity in ItemQuantityList)
            {
                Total += itemQuantity.Quantity * itemQuantity.Item.Price;
            }

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
