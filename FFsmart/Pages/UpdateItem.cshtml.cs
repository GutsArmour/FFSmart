using FFsmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FFsmart.Pages
{
    [Authorize(Roles = "Admin,HeadChef,Chef")]
    public class UpdateItemModel : PageModel
    {
        public readonly AppDataContext _db;

        [BindProperty, Required]
        public Item Item { get; set; }

        public UpdateItemModel(AppDataContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            Item = _db.Items.Find(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) { return Page(); }
            else
            {
                _db.Items.Update(Item);
                _db.SaveChanges();

                return RedirectToPage("RestaurantIndex");
            }
        }
    }
}
