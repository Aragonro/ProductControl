using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Infrastructure;
using System.Security.Claims;

namespace ProductControl.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationResult> Create(ApplicationUserDTO userDto);

        Task<ClaimsIdentity> Authenticate(ApplicationUserDTO userDto);

        Task SetInitialData(ApplicationUserDTO adminDto, List<string> roles);

        Task<ApplicationUserDTO> GetUser(ApplicationUserDTO userDto);
        Task<ApplicationUserDTO> GetUserById(string id);
        Task<ApplicationUserDTO> GetUserByEmailAndSecurityStamp(ApplicationUserDTO userDto);
        Task<ApplicationUserDTO> GetUserByEmail(string email);
    }
}
