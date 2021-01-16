using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// Representation of a product item.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Product <see cref="SubCategory"/> ID.
        /// </summary>
        [Required]
        [Display(Name = "Category")]
        public int SubCategoryId { get; set; }

        /// <summary>
        /// Product Name.
        /// </summary>
        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product picture URL address.
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Picture URL")]
        public string Picture { get; set; }

        /// <summary>
        /// Product price in EUR currency.
        /// </summary>
        [Required]
        [Display(Name = "Price (Eur)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Whether was deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Product's SubCategory.
        /// </summary>
        [Display(Name = "Category")]
        public SubCategory SubCategory { get; set; }
    }
}
