using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FFsmart.Models
{
    public class Record
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Item { get; set; }
        public double WeightDifference { get; set; }
        // null = delivery person
        public string? UserId { get; set; }
        // 0 = hard deletion, 1 = hard insertion, 2 = reorder, 3 = delivery, 4 = stock taken
        public int Action { get; set; }
        public DateTime Created { get; set; }

    }
}
