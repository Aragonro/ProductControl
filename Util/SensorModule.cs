using Ninject.Modules;
using ProductControl.BLL.Services;
using ProductControl.BLL.Interfaces;

namespace ProductControl.Util
{
    public class SensorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISensorService>().To<SensorService>();
        }
    }
}