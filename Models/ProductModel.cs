using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductControl.Models
{
    public class ProductModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string SecurityStamp { get; set; }
    }
}