using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.BLL.DTO
{
    public class MonitoringDTO
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int SensorId { get; set; }
    }
}
