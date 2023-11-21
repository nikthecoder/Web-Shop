using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hakimslivs.Data;
using hakimslivs.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace hakimslivs.Pages.Admin.OrderManager
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public List<IdentityRole> Roles { get; set; }

        [BindProperty]
        public Order Order { get; set; }

        public List<SelectListItem> OrderStatuses { get; set; }

        public async Task LoadOrderStatuses()
        {
            OrderStatuses = await _context.OrdersStatuses.Select(os => new SelectListItem
            {
                Value = os.OrderStatusName,
                Text = os.OrderStatusName,
            }).Distinct()
                .ToListAsync();

            SelectListItem none = new SelectListItem
            {
                Value = "Ingen",
                Text = "Ingen"
            };
            OrderStatuses.Insert(0, none);
        }

        public async Task LoadOrder(int? id)
        {
            Order = await _context.Orders.Include(o => o.OrderStatus).FirstOrDefaultAsync(m => m.ID == id);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Roles = await _roleManager.Roles.ToListAsync();

            if (id == null)
            {
                return NotFound();
            }

            await LoadOrderStatuses();

            try
            {
                await LoadOrder(id);
            }
            catch
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, Order order)
        {
            Roles = await _roleManager.Roles.ToListAsync();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            await LoadOrderStatuses();
            await LoadOrder(id);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.PaymentOk = order.PaymentOk;

            try
            {
                Order.OrderStatus = await _context.OrdersStatuses.FirstAsync(o => o.OrderStatusName == order.OrderStatus.OrderStatusName);
            }
            catch
            {
                Order.OrderStatus = null;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(o => o.ID == id);
        }
    }
}
