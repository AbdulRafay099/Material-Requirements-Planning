using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MRP.Models
{
    public class BOM
    {
        [Key]
        public int MaterialId { get; set; }

        [Required]
        public string MaterialName { get; set; }

        [Required]
        public int MaterialPrice { get; set; }

        [Required]
        public int MaterialQty { get; set; }

        public byte[] ItemImage { get; set; }

        public virtual int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Inventory Item { get; set; }

        public virtual int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }


    }
}
