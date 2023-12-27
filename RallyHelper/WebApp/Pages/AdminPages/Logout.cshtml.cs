using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.AdminPages;

public class Logout : PageModel
{
    public IActionResult OnGet()
    {
        HttpContext.Session.SetString("IsAdmin", "false");
        return RedirectToPage("/Index");
    }
}