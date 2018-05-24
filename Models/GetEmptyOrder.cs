using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductControl.Models
{
    public class GetEmptyOrder
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public int CountProduct { get; set; }
        public string CustomerEmail { get; set; }
    }
}