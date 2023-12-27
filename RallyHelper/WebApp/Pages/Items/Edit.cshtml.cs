using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class EditModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public EditModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
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
        Item = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {

        var itemToUpdate = await _context.Items.FirstOrDefaultAsync(i => i.Id == Item.Id);
        if (itemToUpdate == null)
        {
            return NotFound();
        }

        itemToUpdate.CurrentQuantity = Item.CurrentQuantity; // Update only the CurrentQuantity

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ItemExists(Item.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        TempData["Updated"] = true;
        return RedirectToPage("./Index", new { update = DateTime.Now.Ticks });
    }

    private bool ItemExists(int id)
    {
        return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}