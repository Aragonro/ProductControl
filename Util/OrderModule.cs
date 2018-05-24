using Ninject.Modules;
using ProductControl.BLL.Services;
using ProductControl.BLL.Interfaces;

namespace ProductControl.Util
{
    public class OrderModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IOrderService>().To<OrderService>();
        }
    }
}