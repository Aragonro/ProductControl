using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.BLL.DTO
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string SecurityStamp { get; set; }
        public string Role { get; set; }
    }
}
