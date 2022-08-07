using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MRP.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime placedDate { get; set; }

        public DateTime deliveryDate { get; set; }

        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
}
