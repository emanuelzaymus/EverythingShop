using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// ViewModel for ProductsController.
    /// </summary>
    public class SearchProductViewModel
    {
        /// <summary>
        /// MainCategories with their SubCategories.
        /// </summary>
        public List<MainCategory> AllCategories { get; set; }

        /// <summary>
        /// All or filtered products based on <see cref="PriceFrom"/>, <see cref="PriceTo"/> 
        /// and <see cref="SearchString"/> of <see cref="SubCategory"/> with <see cref="SubCategoryId"/>.
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// Filter <see cref="Products"/> based on this <see cref="SubCategory"/>.
        /// </summary>
        public int? SubCategoryId { get; set; }

        /// <summary>
        /// Minimum price filter condition.
        /// </summary>
        [Display(Name = "Price From")]
        public int? PriceFrom { get; set; }

        /// <summary>
        /// Maximum price filter condition.
        /// </summary>
        [Display(Name = "Price To")]
        public int? PriceTo { get; set; }

        /// <summary>
        /// Search condition in product names based on this string.
        /// </summary>
        [Display(Name = "Search products")]
        public string SearchString { get; set; }
    }
}
