using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Items
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

      public Item Item { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            else 
            {
                Item = item;
            }
            return Page();
        }
    }
}
