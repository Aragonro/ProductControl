using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.BLL.DTO
{
    public class SensorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWorking { get; set; }
        public bool IsProduct { get; set; }
        public int CountProduct { get; set; }
        public string DeliveryAddress { get; set; }
        public bool AutoDelivery { get; set; }
        public string ApplicationUserId { get; set; }
        public int ProductId { get; set; }
    }
}
