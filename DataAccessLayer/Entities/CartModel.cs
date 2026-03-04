using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("Cart")]
    public class CartModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }

        // From the puppy table (not in cart table)
        [NotMapped]
        public string name { get; set; }
        [NotMapped]
        public string breed { get; set; }
        [NotMapped]
        public decimal fee { get; set; }
        [NotMapped]
        public string profile_pic { get; set; }
    }

}