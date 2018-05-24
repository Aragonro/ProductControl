using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.Dal.Entities;
using ProductControl.Dal.Identity;

namespace ProductControl.Dal.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
    
        ApplicationRoleManager RoleManager { get; }

        IRepository<Monitoring> Monitorings { get; }

        IRepository<Product> Products { get; }

        IRepository<Sensor> Sensors { get; }

        IRepository<Order> Orders { get; }

        void Save();
    }
}
