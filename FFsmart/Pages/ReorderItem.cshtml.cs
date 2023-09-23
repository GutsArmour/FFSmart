using FFsmart.Models;
using FFsmart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FFsmart.Pages
{
    [Authorize(Roles = "HeadChef,Chef")]
    public class ReorderItemModel : PageModel
    {
        public readonly AppDataContext _db;
        public readonly UserManager<AppUser> _userManager;

        [BindProperty, Required]
        public Item Item { get; set; }

        public Order PendingOrder { get; set; }

        public List<Order> Orders { get; set; }

        public ReorderItemModel(AppDataContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;

            Orders = _db.Orders.ToList();

            foreach (var order in _db.Orders.ToList())
            {
                if (order.IsSubmitted == false)
                {
                    PendingOrder = order;
                }
            }
        }

        public void OnGet(int id)
        {
            Item = _db.Items.Find(id);
        }

        public IActionResult OnPost()
        {
            Item.OrderId = PendingOrder.Id;

            Record Record = new Record
            {
                Item = Item.Name,
                WeightDifference = Item.Weight,
                UserId = _userManager.GetUserId(HttpContext.User),
                Action = 2,
                Created = DateTime.UtcNow
            };
            _db.Records.Add(Record);

            _db.Items.Update(Item);

            _db.SaveChanges();

            return RedirectToPage("RestaurantIndex");
        }
    }
}
