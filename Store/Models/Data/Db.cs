using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Store.Models.Data
{
    public class Db :DbContext
    {
        public DbSet<PagesProduct> Products { get; set; }
        public DbSet<CategoryProduct> Categories { get; set; }
    }
}