using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Store.Models.Data
{
    [Table("tblOrders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public  DateTime CreatedAt { get; set; }
        [ForeignKey("UserId")]
        public virtual User Users { get; set; }
    }
}