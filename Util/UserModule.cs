using Ninject.Modules;
using ProductControl.BLL.Services;
using ProductControl.BLL.Interfaces;

namespace ProductControl.Util
{
    public class UserModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
        }
    }
}