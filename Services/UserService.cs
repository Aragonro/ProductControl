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
using System.Security.Claims;

namespace ProductControl.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationResult> Create(ApplicationUserDTO userDto)
        {
            if(userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User is null");
            }
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = userDto.Email,
                    UserName = userDto.Email,
                    FirstName = userDto.FirstName,
                    SecondName = userDto.SecondName,
                    Orders = new List<Order>(),
                    Sensors = new List<Sensor>(),
                    Monitorings = new List<Monitoring>(),
                };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationResult(result.Errors.FirstOrDefault());
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                Database.Save();
                return new OperationResult("Registration is successfully finished");
            }
            else
            {
                return new OperationResult("User with this email already exists");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(ApplicationUserDTO userDto)
        {
            ClaimsIdentity claim = null;

            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task SetInitialData(ApplicationUserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public async Task<ApplicationUserDTO> GetUser(ApplicationUserDTO userDto)
        {
            if(userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User is null");
            }
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user == null)
            {
                throw new Exception("User is not found");
            }
            string roleId = (await Database.UserManager.GetRolesAsync(user.Id)).ToList()[0];
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, ApplicationUserDTO>()).CreateMapper();
            ApplicationUserDTO applicationUserDTO = mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
            applicationUserDTO.Role = roleId;
            return applicationUserDTO;
        }
        public async Task<ApplicationUserDTO> GetUserByEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email), "Email is null");
            }
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User is not found");
            }
            string roleId = (await Database.UserManager.GetRolesAsync(user.Id)).ToList()[0];
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, ApplicationUserDTO>()).CreateMapper();
            ApplicationUserDTO applicationUserDTO = mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
            applicationUserDTO.Role = roleId;
            return applicationUserDTO;
        }

        public async Task<ApplicationUserDTO> GetUserById(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "User id is null");
            }
            ApplicationUser user = await Database.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User is not found");
            }
            string roleId = (await Database.UserManager.GetRolesAsync(user.Id)).ToList()[0];
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, ApplicationUserDTO>()).CreateMapper();
            ApplicationUserDTO applicationUserDTO = mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
            applicationUserDTO.Role = roleId;
            return applicationUserDTO;
        }

        public async Task<ApplicationUserDTO> GetUserByEmailAndSecurityStamp(ApplicationUserDTO userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User is null");
            }
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                throw new Exception("User is not found");
            }
            if (userDto.SecurityStamp != user.SecurityStamp)
            {
                throw new Exception("Email or Security Stamp is wrong");
            }
            string role = (await Database.UserManager.GetRolesAsync(user.Id)).ToList()[0];
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, ApplicationUserDTO>()).CreateMapper();
            ApplicationUserDTO applicationUserDTO = mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
            applicationUserDTO.Role = role;
            applicationUserDTO.Password = null;
            return applicationUserDTO;
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
