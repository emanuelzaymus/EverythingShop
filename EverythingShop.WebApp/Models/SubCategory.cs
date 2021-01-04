using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MainCategoryId { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        public virtual MainCategory MainCategory { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
