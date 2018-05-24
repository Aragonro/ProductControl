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
    public class SensorService : ISensorService
    {
        IUnitOfWork Database { get; set; }

        public SensorService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationResult CreateSensor(SensorDTO sensorDto)
        {
            if (sensorDto == null)
            {
                throw new ArgumentNullException(nameof(sensorDto), "Sensor is null");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SensorDTO, Sensor>()).CreateMapper();
            Sensor sensor = mapper.Map<SensorDTO, Sensor>(sensorDto);
            sensor.Orders = new List<Order>();

            Database.Sensors.Create(sensor);
            Database.Save();
            Sensor mySensor = Database.Sensors.GetAll().Last();
            Monitoring monitoring =new Monitoring {
                SensorId =mySensor.Id,
                ApplicationUserId =mySensor.ApplicationUserId
            };
            Database.Monitorings.Create(monitoring);
            Database.Save();
            return new OperationResult("Sensor was created");
        }

        public SensorDTO GetSensorById(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Sensor id is null");
            }

            Sensor sensor = Database.Sensors.Get(id.Value);
            if (sensor == null)
            {
                throw new Exception("Sensor is not found");

            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Sensor, SensorDTO>()).CreateMapper();
            return mapper.Map<Sensor, SensorDTO>(sensor);
        }

        public IEnumerable<SensorDTO> GetSensorsByUserId(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "User id is null");
            }

            var sensors = Database.Sensors.Find(i => i.ApplicationUserId == id).ToList();
            if (sensors.Count == 0)
            {
                throw new Exception("User doesn't have sensors");

            }
            List<SensorDTO> sensorsDTO = new List<SensorDTO>();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Sensor, SensorDTO>()).CreateMapper();
            foreach (Sensor sensor in sensors)
            {
                sensorsDTO.Add(mapper.Map<Sensor, SensorDTO>(sensor));
            }
            return sensorsDTO;
        }

        public IEnumerable<SensorDTO> GetSensorsWithoutUser()
        {
            var sensors = Database.Sensors.Find(i => i.ApplicationUserId == null).ToList();
            List<SensorDTO> sensorsDTO = new List<SensorDTO>();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Sensor, SensorDTO>()).CreateMapper();
            foreach (Sensor sensor in sensors)
            {
                sensorsDTO.Add(mapper.Map<Sensor, SensorDTO>(sensor));
            }
            return sensorsDTO;
        }
        public OperationResult EditSensor(SensorDTO sensorDTO)
        {
            if (sensorDTO == null)
            {
                throw new ArgumentNullException(nameof(sensorDTO), "Sensor is null");
            }

            Sensor sensor = Database.Sensors.Get(sensorDTO.Id);

            if (sensor == null)
            {
                throw new Exception("Sensor is not found");

            }
            sensor.IsProduct = sensorDTO.IsProduct;
            sensor.IsWorking = sensorDTO.IsWorking;
            sensor.ProductId = sensorDTO.ProductId;
            sensor.ApplicationUserId = sensorDTO.ApplicationUserId;
            sensor.CountProduct = sensorDTO.CountProduct;
            sensor.DeliveryAddress = sensorDTO.DeliveryAddress;
            sensor.Name = sensorDTO.Name;
            sensor.AutoDelivery = sensorDTO.AutoDelivery;
            Database.Sensors.Update(sensor);
            Database.Save();
            return new OperationResult("Sensor was edited");
        }

        public OperationResult SetOwner(string idOwner, int? idSensor)
        {
            if (idOwner == null)
            {
                throw new ArgumentNullException(nameof(idOwner), "User id is null");
            }

            if (idSensor == null)
            {
                throw new ArgumentNullException(nameof(idSensor), "Sensor id is null");
            }

            var users = Database.UserManager.Users.Where(c => c.Id == idOwner).ToList();
            if (users == null)
            {
                throw new Exception("User is not found");

            }
            var user = users[0];
            Sensor sensor = Database.Sensors.Get(idSensor.Value);
            if (sensor == null)
            {
                throw new Exception("Sensor is not found");
            }

            sensor.ApplicationUserId = user.Id;
            sensor.Orders = new List<Order>();
            return new OperationResult("Sensor was had owner");
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
