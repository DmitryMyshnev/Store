using Store.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models.ViewModels.Shop
{
    public class CategoryVM
    {
        public CategoryVM(){ }
        public CategoryVM(CategoryProduct row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}