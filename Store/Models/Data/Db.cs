﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Store.Models.Data
{
    public class Db :DbContext
    {
        public DbSet<PagesDTO> Pages { get; set; }
    }
}