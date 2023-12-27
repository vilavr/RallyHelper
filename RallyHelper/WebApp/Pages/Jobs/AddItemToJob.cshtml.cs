using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Jobs;

public class AddItemToJobModel : PageModel
{
    private readonly AppDbContext _context;

    public AddItemToJobModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public int ItemId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int JobId { get; set; }

    [BindProperty]
    public int Quantity { get; set; }
    
    [BindProperty]
    public int MaxAvailableQuantity { get; set; }

    public async Task OnGetAsync()
    {
        Console.WriteLine("Hello from get on add item");
        var item = await _context.Items
            .Where(i => i.Id == ItemId)
            .FirstOrDefaultAsync();
        MaxAvailableQuantity = item?.CurrentQuantity ?? 0;
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        // Check if the job and item exist
        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == JobId);
        var itemExists = await _context.Items.AnyAsync(i => i.Id == ItemId);

        if (jobExists && itemExists && Quantity > 0)
        {
            var jobItem = new JobItem
            {
                JobId = JobId,
                ItemId = ItemId,
                NeededQuantity = Quantity
            };

            _context.JobItems.Add(jobItem);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Item added to job successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Error: Invalid job or item, or quantity is zero.";
        }

        return RedirectToPage("/Jobs/AddJobComponents", new { jobId = JobId });
    }

}