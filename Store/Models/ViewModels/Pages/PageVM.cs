using Store.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        { }
        public PageVM(Page row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }

    }
}