using Ninject.Modules;
using ProductControl.BLL.Services;
using ProductControl.BLL.Interfaces;

namespace ProductControl.Util
{
    public class ProductModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IProductService>().To<ProductService>();
        }
    }
}