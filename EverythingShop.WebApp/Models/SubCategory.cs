using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class SubCategory : ICategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Main Category")]
        public int MainCategoryId { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        public virtual MainCategory MainCategory { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
