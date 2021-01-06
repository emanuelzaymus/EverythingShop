using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class SearchProductViewModel
    {
        public List<MainCategory> AllCategories { get; set; }

        public SelectList MainCategories { get; set; }

        public SelectList SubCategories { get; set; }

        public List<Product> Products { get; set; }

        public int? MainCategory { get; set; }

        public int? SubCategory { get; set; }

        public string SearchString { get; set; }
    }
}
