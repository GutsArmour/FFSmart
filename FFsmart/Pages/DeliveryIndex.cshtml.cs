using FFsmart.Models;
using FFsmart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FFsmart.Pages
{
    public class DeliveryIndexModel : PageModel
    {
        public readonly AppDataContext _db;
        public PasscodeService _passcodeService { get; set; }
        public List<Order> SubmittedOrders { get; set; }

        public DeliveryIndexModel(AppDataContext db, PasscodeService passcodeService)
        {
            _db = db;
            _passcodeService = passcodeService;
            SubmittedOrders = _db.Orders.Where(o => o.IsSubmitted == true && o.IsCompleted == false).ToList();
        }
    }
}
