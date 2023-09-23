using FFsmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace FFsmart.Pages
{
    [Authorize(Roles = "Admin")]
    public class UpdateUserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [BindProperty(SupportsGet = true)]
        public string UserID { get; set; }
        public AppUser User { get; set; }
    
        [BindProperty, Required]
        public string NewPassword { get; set; }

        public List<IdentityRole> Roles { get; set; }
        [BindProperty]
        public string NewRole { get; set; }
        public string CurrentRole { get; set; }

        public UpdateUserModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            User = await _userManager.FindByIdAsync(UserID);
            Roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in Roles)
            {
                if (await _userManager.IsInRoleAsync(User, role.Name))
                {
                    CurrentRole = role.NormalizedName;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostPasswordAsync()
        {
            User = await _userManager.FindByIdAsync(UserID);
            Roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in Roles)
            {
                if (await _userManager.IsInRoleAsync(User, role.Name))
                {
                    CurrentRole = role.NormalizedName;
                }
            }

            await _userManager.RemovePasswordAsync(User);
            await _userManager.AddPasswordAsync(User, NewPassword);
            return RedirectToPage("/Admin/Admin");
        }

        public async Task<IActionResult> OnPostRoleAsync()
        {
            User = await _userManager.FindByIdAsync(UserID);
            Roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in Roles)
            {
                if (await _userManager.IsInRoleAsync(User, role.Name))
                {
                    CurrentRole = role.NormalizedName;
                }
            }

            if (CurrentRole != null) { await _userManager.RemoveFromRoleAsync(User, CurrentRole); }
            await _userManager.AddToRoleAsync(User, NewRole);
            
            return RedirectToPage("/Admin/Admin");
        }

        public async Task<IActionResult> OnGetDeleteAsync()
        {
            if (UserID != null)
            {
                User = await _userManager.FindByIdAsync(UserID);
                await _userManager.DeleteAsync(User);
            }
            return RedirectToPage("/Admin/Admin");
        }
    }
}
