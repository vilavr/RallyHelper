using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Domain;

namespace WebApp.Pages.Jobs
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public bool HasActiveRally { get; set; }

        public void OnGet()
        {
            HasActiveRally = _context.Rallies.Any(r => r.EndTime == null || r.EndTime > DateTime.Now);
        }

        [BindProperty]
        public string ClientName { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(ClientName))
            {
                return Page();
            }

            // Find the active rally
            var activeRally = _context.Rallies
                .FirstOrDefault(r => r.StartTime <= DateTime.Now && 
                                     (r.EndTime == null || r.EndTime > DateTime.Now));

            if (activeRally == null)
            {
                TempData["Message"] = "No active rally available. Cannot create a job.";
                return RedirectToPage();
            }

            var job = new Job
            {
                ClientName = ClientName,
                RallyId = activeRally.Id 
                // Other properties remain null initially
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            // Redirect to the next step with the job's ID
            return RedirectToPage("/Jobs/AddJobComponents", new { jobId = job.Id });
        }

    }
}