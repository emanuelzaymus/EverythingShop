using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// Main Category that contains SubCategories.
    /// </summary>
    public class MainCategory : ICategory
    {
        /// <summary>
        /// ID of <see cref="MainCategory"/>.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// MainCategory name.
        /// </summary>
        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        /// <summary>
        /// Whether was deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// MainCategory's Subcategories.
        /// </summary>
        public virtual List<SubCategory> SubCategories { get; set; }

        /// <summary>
        /// String represantion.
        /// </summary>
        /// <returns>Name of Maincategory.</returns>
        public override string ToString() => Name;
    }
}
