using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFsmart.Models
{
    public class Item
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Item ID")]
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Weight (kg)")]
        public double Weight { get; set; } //kg
        public DateTime DeliveryDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        [DisplayName("Order ID")]
        public int OrderId { get; set; }
    }
}
