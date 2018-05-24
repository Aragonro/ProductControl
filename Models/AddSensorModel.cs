using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductControl.Models
{
    public class AddSensorModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Product { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public string EmailAdmin { get; set; }
        [Required]
        public string SecurityStamp { get; set; }
    }
}