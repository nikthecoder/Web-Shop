using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using hakimslivs.Data;
using hakimslivs.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hakimslivs.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Item> Items { get;set; }
        public IList<Category> Categories { get;set; }
        public List<SelectListItem> Sorting { get; set; }
        [FromQuery]
        public string Sort { get; set; }
        [FromQuery]
        public string CurrentCategory { get; set; }
        [FromQuery]
        public string SearchTerm { get; set; }

        public void LoadSorting()
        {
            Sorting = new List<SelectListItem>();
            SelectListItem sortera = new SelectListItem
            {
                Value = "",
                Text = "Sortera"
            };
            SelectListItem priceLow = new SelectListItem
            {
                Value = "lowest",
                Text = "Lägsta Pris"
            };
            SelectListItem priceHigh = new SelectListItem
            {
                Value = "highest",
                Text = "Högsta Pris"
            };
            Sorting.Insert(0, priceHigh);
            Sorting.Insert(0, priceLow);
            Sorting.Insert(0, sortera);
        }

        public async Task OnGetAsync()
        {
            LoadSorting();
            IQueryable<Item> query = _context.Items.Include(i => i.Category);
            Categories = await _context.Categories.ToListAsync();

            if (!String.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(p => p.Product.ToLower().Contains(SearchTerm.ToLower()) || p.Category.Name.ToLower().Contains(SearchTerm.ToLower()) || p.Description.ToLower().Contains(SearchTerm.ToLower()) || p.Price.ToString().Contains(SearchTerm) || p.Stock.ToString().Contains(SearchTerm));
            }

            if (!String.IsNullOrEmpty(CurrentCategory))
            {
                query = query.Where(i => i.Category.Name == CurrentCategory);
            }

            if (!String.IsNullOrEmpty(Sort))
            {
                if (Sort == "lowest")
                {
                    query = query.OrderBy(i => i.Price);
                }
                else if (Sort == "highest")
                {
                    query = query.OrderByDescending(i => i.Price);
                }
            }

            Items = await query.ToListAsync();
        }
    }
}
