using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class PuppyModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int product_id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public int age { get; set; }

        public string breed { get; set; }

        public int shelter_id { get; set; }

        public bool cookie { get; set; }

        public string description { get; set; }

        public int fee { get; set; }
    }
}
