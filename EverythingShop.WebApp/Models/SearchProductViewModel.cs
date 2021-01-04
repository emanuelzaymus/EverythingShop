using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class SearchProductViewModel
    {
        public SelectList MainCategories { get; set; }
        public SelectList SubCategories { get; set; }
        public List<Product> Products { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string SearchString { get; set; }
    }
}
