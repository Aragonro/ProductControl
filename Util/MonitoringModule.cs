using Ninject.Modules;
using ProductControl.BLL.Services;
using ProductControl.BLL.Interfaces;

namespace ProductControl.Util
{
    public class MonitoringModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMonitoringService>().To<MonitoringService>();
        }
    }
}