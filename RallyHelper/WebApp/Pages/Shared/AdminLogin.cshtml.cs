using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace WebApp.Pages.Shared;

public class AdminLogin : PageModel
{
    [BindProperty]
    public string Username { get; set; }
    
    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public IActionResult OnPost()
    {
        if (Username == "cyber" && Password == "security")
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            return RedirectToPage("/Jobs/Index"); // Replace with the actual admin dashboard page
        }
        else
        {
            ErrorMessage = "Invalid username or password.";
            return Page();
        }
    }

    public void OnGet()
    {
        
    }
}