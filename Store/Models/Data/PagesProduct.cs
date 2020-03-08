using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Store.Models.Data
{   [Table("tblProducts")]
    public class PagesProduct
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }     
        public string Body { get; set; }
        public decimal Price { get; set; }
       
    }
}