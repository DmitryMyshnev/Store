using Store.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        { }
        public PageVM(PagesProduct row)
        {
            Id = row.Id;
            Title = row.Title;         
            Body = row.Body;
            Price = row.Price;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(int.MaxValue,MinimumLength =3)]
        public string Body { get; set; }
        [Required]
        public decimal Price { get; set; }
       
    }
}