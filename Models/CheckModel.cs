using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductControl.Models
{
    public class CheckModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string SecurityStamp { get; set; }
    }
}