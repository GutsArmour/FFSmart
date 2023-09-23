using FFsmart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FFsmart.Pages
{
    public class OrderModel : PageModel
    {
        public readonly AppDataContext _db;
        public Order Order { get; set; }
        public List<Item> OrderItems { get; set; }

        [BindProperty, Required]
        public DateTime NewExpDate { get; set; }
        public OrderModel(AppDataContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            Order = _db.Orders.Find(id);
            OrderItems = _db.Items.Where(i => i.OrderId == Order.Id).ToList();
        }

        public IActionResult OnPostSetDate(int id)
        {
            var item = _db.Items.Find(id);

            if (item != null)
            {
                item.ExpirationDate = NewExpDate;
                _db.Items.Update(item);
                _db.SaveChanges();
            }

            return RedirectToPage("Order", new { id = item.OrderId });
        }

        public IActionResult OnPostConfirmDelivery(int id)
        {
            Order = _db.Orders.Find(id);
            OrderItems = _db.Items.Where(i => i.OrderId == Order.Id).ToList();

            foreach (var item in OrderItems)
            {
                Record Record = new Record
                {
                    Item = item.Name,
                    WeightDifference = item.Weight,
                    UserId = null,
                    Action = 3,
                    Created = DateTime.UtcNow
                };
                _db.Records.Add(Record);

                item.OrderId = 0;
                item.DeliveryDate = DateTime.UtcNow;

                _db.Items.Update(item);
                _db.SaveChanges();
            }

            Order.IsCompleted = true;
            _db.Orders.Update(Order);
            _db.SaveChanges();
            
            return RedirectToPage("DeliveryIndex");
        }
    }
}
