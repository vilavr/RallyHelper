using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DbOperations; // Assuming this is the namespace for your InsertStartData class

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context; // Add AppDbContext

    public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        // Call InsertStartData method to populate the database with initial data
        var dataInserter = new InsertStartData(_context);
        dataInserter.InsertData();
    }
}
