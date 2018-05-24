using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.Dal.Entities;
using ProductControl.Dal.EF;
using ProductControl.Dal.Interfaces;
using ProductControl.Dal.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProductControl.Dal.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ProductControlContext db;

        private SensorRepository sensorRepository;
        private ProductRepository productRepository;
        private OrderRepository orderRepository;
        private MonitoringRepository monitoringRepository;
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;

        public EFUnitOfWork(string connectionString)
        {
            db = new ProductControlContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
        }
        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public IRepository<Sensor> Sensors
        {
            get
            {
                if (sensorRepository == null)
                    sensorRepository = new SensorRepository(db);
                return sensorRepository;
            }
        }

        public IRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(db);
                return productRepository;
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }
        }

        public IRepository<Monitoring> Monitorings
        {
            get
            {
                if (monitoringRepository == null)
                    monitoringRepository = new MonitoringRepository(db);
                return monitoringRepository;
            }
        }


        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

