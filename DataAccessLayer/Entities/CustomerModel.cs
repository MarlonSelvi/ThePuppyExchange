using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class CustomerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int id { get; set; } 
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }

        public string password { get; set; }

        public bool cookie { get; set; }
    }
}
