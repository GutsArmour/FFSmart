using FFsmart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FFsmart.Pages
{
    public class RecordsModel : PageModel
    {
        public readonly AppDataContext _db;
        public readonly UserManager<AppUser> _userManager;

        public List<Record> Records { get; set; }
        public List<AppUser> Users { get; set; }

        public RecordsModel(AppDataContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            Records = _db.Records.ToList();
            Users = await _userManager.Users.ToListAsync();
        }
    }
}
