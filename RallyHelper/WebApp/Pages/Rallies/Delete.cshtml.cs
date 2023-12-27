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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Rallies == null)
            {
                return NotFound();
            }
            var rally = await _context.Rallies.FindAsync(id);

            if (rally != null)
            {
                Rally = rally;
                _context.Rallies.Remove(Rally);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
