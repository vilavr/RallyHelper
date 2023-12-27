using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Jobs;

public class ReviewOrder : PageModel
{
    private readonly AppDbContext _context;

    public ReviewOrder(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public int JobId { get; set; }

    public List<JobItem> JobItems { get; set; }

    public decimal TotalPrice { get; set; }

    public async Task OnGetAsync()
    {
        JobItems = await _context.JobItems
            .Where(ji => ji.JobId == JobId)
            .Include(ji => ji.Item)
            .ToListAsync();

        // Calculate the total price
        TotalPrice = JobItems.Sum(ji => ji.Item.PricePerItem * ji.NeededQuantity);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int itemId)
    {
        var jobItem = await _context.JobItems
            .FirstOrDefaultAsync(ji => ji.JobId == JobId && ji.ItemId == itemId);

        if (jobItem != null)
        {
            _context.JobItems.Remove(jobItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage(new { jobId = JobId });
    }
    
    public async Task<IActionResult> OnPostPlaceOrderAsync()
    {
        var job = await _context.Jobs.Include(j => j.JobItems).ThenInclude(ji => ji.Item).FirstOrDefaultAsync(j => j.Id == JobId);

        // Check if all items have sufficient quantity in stock
        foreach (var jobItem in job.JobItems)
        {
            if (jobItem.Item.CurrentQuantity < jobItem.NeededQuantity)
            {
                TempData["ErrorMessage"] = $"Insufficient stock for item: {jobItem.Item.ItemName}.";
                return RedirectToPage(new { jobId = JobId });
            }
        }

        // Deduct the quantity from stock and calculate the total price
        decimal totalPrice = 0;
        foreach (var jobItem in job.JobItems)
        {
            jobItem.Item.CurrentQuantity -= jobItem.NeededQuantity;
            totalPrice += jobItem.Item.PricePerItem * jobItem.NeededQuantity;
        }

        // Update job details and save changes
        job.StartTime = DateTime.Now;
        job.TotalPrice = totalPrice;
        await _context.SaveChangesAsync();

        return RedirectToPage("/Jobs/Index");
    }

}