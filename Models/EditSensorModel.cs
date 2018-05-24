using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductControl.Models
{
    public class EditSensorModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ObserverEmail { get; set; }
        [Required]
        public int CountProduct { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public bool IsWorking { get; set; }
        [Required]
        public bool AutoDelivery { get; set; }
        [Required]
        public string EmailAdmin { get; set; }
        [Required]
        public string SecurityStamp { get; set; }
    }
}