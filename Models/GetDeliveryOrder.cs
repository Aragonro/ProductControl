using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductControl.Models
{
    public class GetDeliveryOrder
    {
        public string DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Price { get; set; }

    }
}