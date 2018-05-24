using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProductControl.Dal.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Sensor> Sensors { get; set; }
        public virtual ICollection<Monitoring> Monitorings { get; set; }
    }
}
