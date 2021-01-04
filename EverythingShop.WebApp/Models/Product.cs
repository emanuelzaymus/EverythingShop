using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}
