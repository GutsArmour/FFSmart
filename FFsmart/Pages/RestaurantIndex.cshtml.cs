using FFsmart.Models;
using FFsmart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace FFsmart.Pages
{
    [Authorize(Roles = "Admin,HeadChef,Chef")]
    public class RestaurantIndexModel : PageModel
    {
        public readonly AppDataContext _db;
        public readonly UserManager<AppUser> _userManager;
        public readonly PasscodeService _passcodeService;

        [BindProperty(SupportsGet = true)]
        public List<Item> Items { get; set; }

        [BindProperty(SupportsGet = true)]
        public Order PendingOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Item> PendingOrderItems { get; set; }

        [BindProperty, Required]
        public int ReduceStockBy { get; set; }

        public RestaurantIndexModel(AppDataContext db, UserManager<AppUser> userManager, PasscodeService passcodeService)
        {
            _db = db;
            _userManager = userManager;
            _passcodeService = passcodeService;
        }

        public void OnGet(string? passcode)
        {
            if (Items.Count == 0)
            {
                Items = _db.Items.ToList();
            }

            LoadPendingOrder();

            if (passcode != null)
            {
                ViewData["Passcode"] = string.Format("Passcode for Delivery Company: " + passcode);
            }
        }

        public void OnGetCloseToExpiry()
        {
            DateTime WarningDate = DateTime.UtcNow.AddDays(4);
            Items = _db.Items.Where(i => i.ExpirationDate < WarningDate && i.ExpirationDate >= DateTime.UtcNow).ToList();
            LoadPendingOrder();
        }

        public void OnGetExpired()
        {
            Items = _db.Items.Where(i => i.ExpirationDate < DateTime.UtcNow).ToList();
            LoadPendingOrder();
        }

        public void OnGetLowOnStock()
        {
            Items = _db.Items.Where(i => i.Weight < 5).ToList();
            LoadPendingOrder();
        }

        public IActionResult OnGetCreateOrder()
        {
            if (PendingOrder.Id == 0)
            {
                PendingOrder = new();

                PendingOrder.IsCompleted = false;
                PendingOrder.IsSubmitted = false;
                
                var GeneratedPasscode = _passcodeService.GeneratePasscode();
                PendingOrder.Passcode = _passcodeService.HashPasscode(GeneratedPasscode);

                _db.Orders.Add(PendingOrder);
                _db.SaveChanges();

                return RedirectToPage("RestaurantIndex", new { passcode = GeneratedPasscode });
            }

            return RedirectToPage("RestaurantIndex");
        }

        public IActionResult OnGetDeleteItem(int id)
        {
            if (id > 0)
            {
                var item = _db.Items.Find(id);

                if (item != null)
                {
                    Record Record = new Record
                    {
                        Item = item.Name,
                        WeightDifference = item.Weight,
                        UserId = _userManager.GetUserId(HttpContext.User),
                        Action = 0,
                        Created = DateTime.UtcNow
                    };
                    _db.Records.Add(Record);

                    _db.Remove(item);

                    _db.SaveChanges();
                }
            }

            return RedirectToPage("RestaurantIndex");
        }

        public IActionResult OnGetSubmitOrder()
        {
            LoadPendingOrder();
            if (PendingOrder.Id != 0 && PendingOrderItems.Count > 0)
            {
                PendingOrder.IsSubmitted = true;
                _db.Orders.Update(PendingOrder);
                _db.SaveChanges();
                return RedirectToPage("RestaurantIndex", PendingOrder.Passcode);
            }

            return RedirectToPage("RestaurantIndex");
        }

        public IActionResult OnPostTakeStock(int id)
        {
            var item = _db.Items.Find(id);

            if (ReduceStockBy >= 0)
            {
                if (ReduceStockBy > item.Weight) { return RedirectToPage("RestaurantIndex"); }
                else
                {
                    Record Record = new Record
                    {
                        Item = item.Name,
                        WeightDifference = ReduceStockBy,
                        UserId = _userManager.GetUserId(HttpContext.User),
                        Action = 4,
                        Created = DateTime.UtcNow
                    };
                    _db.Records.Add(Record);

                    item.Weight -= ReduceStockBy;
                    _db.Items.Update(item);
                    _db.SaveChanges();
                }
            }
            
            return RedirectToPage("RestaurantIndex");
        }

        public void LoadPendingOrder()
        {
            foreach (var order in _db.Orders.ToList())
            {
                if (order.IsSubmitted == false)
                {
                    PendingOrder = order;
                }
            }

            if (PendingOrder != null)
            {
                var items = new List<Item>();
                foreach (var item in _db.Items.ToList())
                {
                    if (item.OrderId == PendingOrder.Id)
                    {
                        items.Add(item);
                    }
                }
                PendingOrderItems = items;
            }
        }
    }
}
