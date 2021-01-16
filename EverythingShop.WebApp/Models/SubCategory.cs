using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// SubCategory that contains Products.
    /// </summary>
    public class SubCategory : ICategory
    {
        /// <summary>
        /// ID of this <see cref="SubCategory"/>.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// <see cref="MainCategory"/> ID that this <see cref="SubCategory"/> belongs to.
        /// </summary>
        [Required]
        [Display(Name = "Main Category")]
        public int MainCategoryId { get; set; }

        /// <summary>
        /// Name of <see cref="SubCategory"/>.
        /// </summary>
        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        /// <summary>
        /// Whether was deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Owner of this <see cref="SubCategory"/>.
        /// </summary>
        public virtual MainCategory MainCategory { get; set; }

        /// <summary>
        /// Products in this <see cref="SubCategory"/>.
        /// </summary>
        public virtual List<Product> Products { get; set; }
    }
}
