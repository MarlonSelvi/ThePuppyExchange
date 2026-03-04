using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("AccountPrivileges")]
    public class UserPrivilegeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("privilege")]
        public required string privilege { get; set; }

        [Column("customer_id")]
        public int customer_Id { get; set; }
    }
}