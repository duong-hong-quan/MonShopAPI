using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int AccountId {  get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [NotMapped]
        public double Total {  get; set; }
      
    }
}
