﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class MainCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        public virtual List<SubCategory> SubCategories { get; set; }
    }
}