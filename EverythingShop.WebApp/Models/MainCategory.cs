using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class MainCategory : ICategory
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        public virtual List<SubCategory> SubCategories { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
