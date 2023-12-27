using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Jobs
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Job> Job { get; set; } = default!;
        public bool IsAdmin { get; set; } = false; 

        public async Task OnGetAsync()
        {
            IsAdmin = HttpContext.Session.GetString("IsAdmin") == "true"; // Determine if the user is an admin

            if (_context.Jobs != null)
            {
                Job = await _context.Jobs
                    .Include(j => j.JobItems)
                    .ThenInclude(ji => ji.Item)
                    .ThenInclude(i => i.Category)
                    .Include(j => j.JobItems)
                    .ThenInclude(ji => ji.Item)
                    .ThenInclude(i => i.Location)
                    .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostMarkAsClosedAsync(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job != null && job.EndTime == null)
            {
                job.EndTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}