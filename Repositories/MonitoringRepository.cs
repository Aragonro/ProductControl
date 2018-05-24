using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.Dal.Entities;
using ProductControl.Dal.EF;
using ProductControl.Dal.Interfaces;
using System.Data.Entity;

namespace ProductControl.Dal.Repositories
{
    public class MonitoringRepository : IRepository<Monitoring>
    {
        private ProductControlContext db;

        public MonitoringRepository(ProductControlContext context)
        {
            this.db = context;
        }

        public IEnumerable<Monitoring> GetAll()
        {
            return db.Monitorings;
        }

        public Monitoring Get(int id)
        {
            return db.Monitorings.Find(id);
        }

        public void Create(Monitoring monitoring)
        {
            db.Monitorings.Add(monitoring);
        }

        public void Update(Monitoring monitoring)
        {
            db.Entry(monitoring).State = EntityState.Modified;
        }

        public IEnumerable<Monitoring> Find(Func<Monitoring, Boolean> predicate)
        {
            return db.Monitorings.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Monitoring monitoring = db.Monitorings.Find(id);
            if (monitoring != null)
                db.Monitorings.Remove(monitoring);
        }
    }
}
