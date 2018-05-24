using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductControl.Models
{
    public class SetCourierModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CourierEmail { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public string EmailAdmin { get; set; }
        [Required]
        public string SecurityStamp { get; set; }
    }
}