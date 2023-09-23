using FFsmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FFsmart.Pages
{
    [Authorize(Roles = "Admin,HeadChef,Chef")]
    public class AddItemModel : PageModel
    {
        public readonly AppDataContext _db;
        public readonly UserManager<AppUser> _userManager;

        [BindProperty, Required]
        public Item Item { get; set; }

        public AddItemModel(AppDataContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) { return Page(); }
            else
            {
                Record Record = new Record
                {
                    Item = Item.Name,
                    WeightDifference = Item.Weight,
                    UserId = _userManager.GetUserId(HttpContext.User),
                    Action = 1,
                    Created = DateTime.UtcNow
                };
                _db.Records.Add(Record);

                _db.Items.Add(Item);

                _db.SaveChanges();
                return RedirectToPage("RestaurantIndex");
            }
        }
    }
}
