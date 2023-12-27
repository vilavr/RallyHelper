using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Jobs;

public class AddJobComponents : PageModel
{
    private readonly AppDbContext _context;

    public AddJobComponents(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public int JobId { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string SelectedItemName { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SelectedCategoryId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SelectedLocationId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? NeededQuantity { get; set; }

    public SelectList ItemNameOptions { get; set; }
    public SelectList CategoryOptions { get; set; }
    public SelectList LocationOptions { get; set; }

    public List<Item> FilteredItems { get; set; }

    private async Task PopulateDropdowns()
    {
        ItemNameOptions = new SelectList(await _context.Items.Select(i => i.ItemName).Distinct().ToListAsync(), "", "");
        CategoryOptions = new SelectList(await _context.ItemCategories.ToListAsync(), "Id", "CategoryName");
        LocationOptions = new SelectList(await _context.ItemLocations.ToListAsync(), "Id", "LocationName");
    }

    public async Task OnGetAsync()
    {
        await PopulateDropdowns();
        await ApplyFilters();
    }

    private async Task ApplyFilters()
    {
        IQueryable<Item> query = _context.Items.Include(i => i.Category).Include(i => i.Location);

        if (!string.IsNullOrEmpty(SelectedItemName))
            query = query.Where(i => i.ItemName == SelectedItemName);
        if (SelectedCategoryId.HasValue)
            query = query.Where(i => i.CategoryId == SelectedCategoryId.Value);
        if (SelectedLocationId.HasValue)
            query = query.Where(i => i.LocationId == SelectedLocationId.Value);
        if (NeededQuantity.HasValue)
            query = query.Where(i => i.CurrentQuantity >= NeededQuantity.Value);

        FilteredItems = await query.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await PopulateDropdowns();
        await ApplyFilters();

        return Page();
    }

    public async Task<IActionResult> OnPostClearFiltersAsync()
    {
        return RedirectToPage(new { SelectedItemName = "", SelectedCategoryId = (int?)null, SelectedLocationId = (int?)null, NeededQuantity = (int?)null });
    }
    
    public IActionResult OnPostRedirectToReview(int jobId)
    {
        return RedirectToPage("/Jobs/ReviewOrder", new { jobId = jobId });
    }
}
