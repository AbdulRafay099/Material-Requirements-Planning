using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MRP.Models
{
    public class Inventory
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int ItemPrice { get; set; }

        [Required]
        public int ItemQty { get; set; }
        public byte[] ItemImage { get; set; }

    }
}
