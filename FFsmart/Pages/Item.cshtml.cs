using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FFsmart.Models;

namespace FFsmart.Pages
{
    public class ItemModel : PageModel
    {
        public readonly AppDataContext _db;
        public Item Item { get; set; }
        public List<Item> Items { get; set; }

        public ItemModel(AppDataContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            Items = _db.Items.ToList();
        }
        public IActionResult OnGetDelete(string id)
        {


            _db.Remove(_db.Items.Find(id));
            _db.SaveChanges();

            return RedirectToPage("Item");
        }
    }
}
