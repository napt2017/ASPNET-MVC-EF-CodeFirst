using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_MVC_EF_CodeFirst.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; }

        public virtual Collection<Product> Products { get; set; }
    }
}