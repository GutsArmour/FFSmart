using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFsmart.Models
{
    public class Order
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSubmitted { get; set; }
        public string Passcode { get; set; }
    }
}
