using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.Dal.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProductControl.Dal.EF
{
    public class ProductControlContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<Monitoring> Monitorings { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        static ProductControlContext()
        {
            Database.SetInitializer(new ProductControlDbInitializer());
        }

        public ProductControlContext(string connectionString)
            : base(connectionString)
        {
        }
    }

    public class ProductControlDbInitializer : DropCreateDatabaseIfModelChanges<ProductControlContext>
    {
        protected override void Seed(ProductControlContext db)
        {
            //db.Users.Add(new User { Login = "MyLogin", Password = "qweasd123", FirstName = "Ivan", SecondName = "Popovich" });
            db.Sensors.Add(new Sensor { IsWorking = false, IsProduct = false });
            db.SaveChanges();
        }
    }
}