using FFsmart.Models;
using FFsmart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace FFsmart.Pages
{
    public class OrderAccessModel : PageModel
    {
        public readonly AppDataContext _db;
        public readonly PasscodeService _passcodeService;
        public Order Order { get; set; }
        [BindProperty, Required]
        public string Passcode { get; set; }
        [BindProperty, Required]
        public int OrderId { get; set; }

        public OrderAccessModel(AppDataContext db, PasscodeService passcodeService)
        {
            _db = db;
            _passcodeService = passcodeService;
        }

        public void OnGet(int id)
        {
            Order = _db.Orders.Find(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) { return RedirectToPage("/OrderAccess", new { id = Order.Id }); }
            else
            {
                Order = _db.Orders.Find(OrderId);
                if ( _passcodeService.VerifyHashed(Passcode, Order.Passcode)) { return RedirectToPage("/Order", new { id = Order.Id }); }
               else { return RedirectToPage("/OrderAccess", new { id = Order.Id }); }
            }
        }
    }
}
