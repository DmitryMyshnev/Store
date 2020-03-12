using Store.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        { }
        public PageVM(PagesProduct row)
        {
            Id = row.Id;
            Name = row.Name;         
            Slug = row.Slug;
            Description = row.Description;
            Price = row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3)]
        public string Name { get; set; }      
        [Required]
        [StringLength(int.MaxValue,MinimumLength =3)]
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]   
        public string CategoryName { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }

    }
}