using System.Collections.Generic;

namespace EverythingShop.WebApp.Models
{
    public class SearchProductViewModel
    {
        public List<MainCategory> AllCategories { get; set; }

        public List<Product> Products { get; set; }

        public int? SubCategoryId { get; set; }

        public string SearchString { get; set; }
    }
}
