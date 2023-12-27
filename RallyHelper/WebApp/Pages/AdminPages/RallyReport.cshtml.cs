using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.AdminPages
{
    public class RallyReport : PageModel
    {
        private readonly AppDbContext _context;

        public RallyReport(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int SelectedRallyId { get; set; }
        public List<Rally> Rallies { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Job> Jobs { get; set; }

        public async Task OnGetAsync()
        {
            Rallies = await _context.Rallies.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Rallies = await _context.Rallies.ToListAsync();

            Jobs = await _context.Jobs
                .Where(j => j.RallyId == SelectedRallyId)
                .Include(j => j.JobItems)
                .ThenInclude(ji => ji.Item)
                .ThenInclude(i => i.Category)
                .Include(j => j.JobItems)
                .ThenInclude(ji => ji.Item)
                .ThenInclude(i => i.Location)
                .ToListAsync();

            TotalPrice = (decimal)Jobs.Sum(j => j.TotalPrice);

            return Page();
        }
    }

}