﻿using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int SubCategoryId { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Picture URL")]
        public string Picture { get; set; }

        [Required]
        [Display(Name = "Price (Eur)")]
        public decimal Price { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        [Display(Name = "Category")]
        public SubCategory SubCategory { get; set; }
    }
}
