using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductControl.Models
{
    public class SensorId
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string SecurityStamp { get; set; }
        [Required]
        public int Id { get; set; }

    }
}