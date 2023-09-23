using FFsmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FFsmart.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        // Identity framework managers
        private readonly RoleManager<IdentityRole> _roleManager;
        public readonly UserManager<AppUser> _userManager;

        public List<IdentityRole> Roles { get; set; }
        public List<AppUser> Users { get; set; }

        // To store new role input from form
        [BindProperty, Required]
        public string NewRole { get; set; }

        // ID variables
        [BindProperty(SupportsGet = true)]
        public string RoleId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        public AdminModel(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _roleManager.Roles.ToListAsync();
            Users = await _userManager.Users.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync()
        {
            if (RoleId != null)
            {
                var role = await _roleManager.FindByIdAsync(RoleId);
                await _roleManager.DeleteAsync(role);
            }
            else if (UserId != null)
            {
                var user = await _userManager.FindByIdAsync(UserId);
                await _userManager.DeleteAsync(user);
            }
            return RedirectToPage("/Admin/Admin");
        }

        public async Task<IActionResult> OnPostAddRole()
        {
            if (NewRole != null) {
                await _roleManager.CreateAsync(new IdentityRole(NewRole.Trim()));
            }
            return RedirectToPage("/Admin/Admin");
        }
    }
}
