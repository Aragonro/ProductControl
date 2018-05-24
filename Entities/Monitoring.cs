using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Dal.Entities
{
    public class Monitoring
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int SensorId { get; set; }
    }
}
