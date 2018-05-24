using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductControl.BLL.Interfaces;
using ProductControl.Dal.Interfaces;
using ProductControl.BLL.DTO;
using ProductControl.Dal.Entities;
using Microsoft.AspNet.Identity;
using ProductControl.BLL.Infrastructure;

namespace ProductControl.BLL.Services
{
    public class MonitoringService : IMonitoringService
    {
        IUnitOfWork Database { get; set; }

        public MonitoringService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationResult CreateMonitoring(MonitoringDTO monitoringDto)
        {
            if (monitoringDto == null)
            {
                throw new ArgumentNullException(nameof(monitoringDto), "Monitoring is null");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MonitoringDTO, Monitoring>()).CreateMapper();
            Monitoring monitoring = mapper.Map<MonitoringDTO, Monitoring>(monitoringDto);
            Database.Monitorings.Create(monitoring);
            Database.Save();
            return new OperationResult("Monitoring was created");
        }
        public MonitoringDTO GetMonitoringById(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Monitorng id is null");
            }

            Monitoring monitoring = Database.Monitorings.Get(id.Value);
            if (monitoring == null)
            {
                throw new Exception("Monitorng is not found");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Monitoring, MonitoringDTO>()).CreateMapper();
            MonitoringDTO monitoringDTO = mapper.Map<Monitoring, MonitoringDTO>(monitoring);
            return monitoringDTO;
        }
        public IEnumerable<MonitoringDTO> GetMonitoringsByUserId(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "User id is null");
            }

            var monitorings = Database.Monitorings.Find(i => i.ApplicationUserId == id).ToList();
            if (monitorings.Count == 0)
            {
                throw new Exception("User doesn't have monitorings");

            }
            List<MonitoringDTO> monitoringDTO = new List<MonitoringDTO>();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Monitoring, MonitoringDTO>()).CreateMapper();
            foreach (Monitoring monitoring in monitorings)
            {
                monitoringDTO.Add(mapper.Map<Monitoring, MonitoringDTO>(monitoring));
            }
            return monitoringDTO;
        }
        public MonitoringDTO GetMonitoringsBySensorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Sensor id is null");
            }

            var monitoring = Database.Monitorings.Find(i => i.SensorId == id.Value).FirstOrDefault();
            if (monitoring==null)
            {
                throw new Exception("Sensor doesn't have monitoring");

            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Monitoring, MonitoringDTO>()).CreateMapper();
            MonitoringDTO monitoringDTO = mapper.Map<Monitoring, MonitoringDTO>(monitoring);
            return monitoringDTO;
        }
        public OperationResult EditMonitoring(MonitoringDTO monitoringDto)
        {
            if (monitoringDto == null)
            {
                throw new ArgumentNullException(nameof(monitoringDto), "Monitoring is null");
            }

            Monitoring monitoring = Database.Monitorings.Get(monitoringDto.Id);

            if (monitoring == null)
            {
                throw new Exception("Monitoring is not found");

            }
            monitoring.ApplicationUserId = monitoringDto.ApplicationUserId;
            monitoring.SensorId = monitoringDto.SensorId;
            Database.Monitorings.Update(monitoring);
            Database.Save();
            return new OperationResult("Monitoring was edited");
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
