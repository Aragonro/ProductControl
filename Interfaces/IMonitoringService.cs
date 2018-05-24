using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Infrastructure;

namespace ProductControl.BLL.Interfaces
{
    public interface IMonitoringService
    {
        OperationResult CreateMonitoring(MonitoringDTO monitoringDto);
        MonitoringDTO GetMonitoringById(int? id);
        IEnumerable<MonitoringDTO> GetMonitoringsByUserId(string id);
        MonitoringDTO GetMonitoringsBySensorId(int? id);
        OperationResult EditMonitoring(MonitoringDTO monitoringDto);
        void Dispose();
    }
}
