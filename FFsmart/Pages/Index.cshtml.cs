using FFsmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FFsmart.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDataContext _db;

        public IndexModel(ILogger<IndexModel> logger, UserManager<AppUser> userManager, AppDataContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Restaurant") || User.IsInRole("HeadChef")) { return RedirectToPage("/RestaurantIndex"); }
            else if (User.IsInRole("Delivery")) { return RedirectToPage("/DeliveryIndex"); }
            else if (User.IsInRole("Admin")) { }

            return Page();
        }
    }
}