using System;
using System.ComponentModel.DataAnnotations;
namespace ASPNET_MVC_EF_CodeFirst.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int Year { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public bool IsActive { get; set; }
    }
}