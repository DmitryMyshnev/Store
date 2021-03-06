﻿using Store.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models.ViewModels.Shop
{
    public class OrderVM
    {
        public OrderVM()
        { }
        public OrderVM(Order row)
        {
            OrderId = row.OrderId;
            UserId = row.UserId;
            CreateAt = row.CreatedAt;
        }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateAt { get; set; }

    }
}