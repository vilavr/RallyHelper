using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Rallies
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

      public Rally Rally { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Rallies == null)
            {
                return NotFound();
            }

            var rally = await _context.Rallies.FirstOrDefaultAsync(m => m.Id == id);
            if (rally == null)
            {
                return NotFound();
            }
            else 
            {
                Rally = rally;
            }
            return Page();
        }
    }
}
