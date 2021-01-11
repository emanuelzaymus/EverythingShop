using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EverythingShop.WebApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Picture URL")]
        public string Picture { get; set; }

        [Required]
        public decimal Price { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}
