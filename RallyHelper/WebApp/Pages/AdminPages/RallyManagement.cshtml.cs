using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL; 
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.AdminPages;

public class RallyManagement : PageModel
{
    private readonly AppDbContext _context;

    public RallyManagement(AppDbContext context)
    {
        _context = context;
    }
    public Rally CurrentOrLastRally { get; set; }
    public Dictionary<string, int> ItemUsage { get; set; }
    public Dictionary<string, int> ItemsToRestock { get; set; }

    public void OnGet()
    {
        CurrentOrLastRally = _context.Rallies.OrderByDescending(r => r.StartTime).FirstOrDefault();
        if (CurrentOrLastRally != null)
        {
            var jobs = _context.Jobs
                .Where(j => j.RallyId == CurrentOrLastRally.Id && j.EndTime != null)
                .Include(j => j.JobItems)
                .ThenInclude(ji => ji.Item)
                .ToList();

            // Aggregate item quantities across all jobs
            ItemUsage = new Dictionary<string, int>();
            foreach (var job in jobs)
            {
                foreach (var ji in job.JobItems)
                {
                    if (ItemUsage.ContainsKey(ji.Item.ItemName))
                    {
                        ItemUsage[ji.Item.ItemName] += ji.NeededQuantity;
                    }
                    else
                    {
                        ItemUsage[ji.Item.ItemName] = ji.NeededQuantity;
                    }
                }
            }

            // Calculate items to restock
            ItemsToRestock = new Dictionary<string, int>();
            var allItems = _context.Items.ToList();
            foreach (var item in allItems)
            {
                var totalCurrentQuantity = allItems.Where(i => i.ItemName == item.ItemName).Sum(i => i.CurrentQuantity);
                var totalOptimalQuantity = allItems.Where(i => i.ItemName == item.ItemName).Sum(i => i.OptimalQuantity);
                var restockQuantity = Math.Max(0, totalOptimalQuantity - totalCurrentQuantity);

                if (!ItemsToRestock.ContainsKey(item.ItemName))
                {
                    ItemsToRestock.Add(item.ItemName, restockQuantity);
                }
            }
        }
    }

    public IActionResult OnPostEndCurrentRally()
    {
        var currentRally = _context.Rallies.FirstOrDefault(r => r.EndTime == null);
        if (currentRally == null)
        {
            TempData["Message"] = "No active rally to end.";
            return RedirectToPage();
        }

        currentRally.EndTime = DateTime.Now;
        _context.SaveChanges();

        TempData["Message"] = "Rally ended successfully.";
        return RedirectToPage();
    }

    public IActionResult OnPostStartNewRally()
    {
        if (_context.Rallies.Any(r => r.EndTime == null))
        {
            TempData["Message"] = "Cannot start a new rally while another is active.";
            return RedirectToPage();
        }

        var newRally = new Rally { StartTime = DateTime.Now };
        _context.Rallies.Add(newRally);
        _context.SaveChanges();

        TempData["Message"] = "New rally started successfully.";
        return RedirectToPage();
    }
}