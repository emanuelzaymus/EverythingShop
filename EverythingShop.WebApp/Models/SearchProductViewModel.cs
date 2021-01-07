using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    public class SearchProductViewModel
    {
        public List<MainCategory> AllCategories { get; set; }

        public List<Product> Products { get; set; }

        public int? SubCategoryId { get; set; }

        [Display(Name = "Price From")]
        public int? PriceFrom { get; set; }

        [Display(Name = "Price To")]
        public int? PriceTo { get; set; }

        [Display(Name = "Search products")]
        public string SearchString { get; set; }
    }
}
