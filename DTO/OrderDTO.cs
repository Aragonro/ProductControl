using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.BLL.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool Delivered { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Price { get; set; }
        public int SensorId { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
