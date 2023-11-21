using hakimslivs.Data;
using hakimslivs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hakimslivs.Pages
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderator, Basic")]
    public class InvoiceModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public InvoiceModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public Order Order { get; set; }
        public List<ItemQuantity> ItemQuantities { get; set; }
        public decimal TotalSum { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Thread.Sleep(3000);
            try
            {
                Order = await _context.Orders.Include(o => o.User).FirstAsync(o => o.ID == id);
            }
            catch
            {
                return NotFound();
            }
            ItemQuantities = _context.ItemQuantities.Include(i => i.Order).Include(i => i.Item).Where(i => i.Order == Order).ToList();

            return Page();
        }
    }
}
