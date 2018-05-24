using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Infrastructure;

namespace ProductControl.BLL.Interfaces
{
    public interface ISensorService
    {
        OperationResult CreateSensor(SensorDTO sensorDto);
        IEnumerable<SensorDTO> GetSensorsByUserId(string id);
        IEnumerable<SensorDTO> GetSensorsWithoutUser();
        SensorDTO GetSensorById(int? id);
        OperationResult EditSensor(SensorDTO sensorDto);
        OperationResult SetOwner(string idOwner, int? idSensor);
        void Dispose();
    }
}
