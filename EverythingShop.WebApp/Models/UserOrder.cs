﻿using EverythingShop.WebApp.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class UserOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string ContactName { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string StreetAddress { get; set; }

        [Required, MaxLength(10), MinLength(3), DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required, MaxLength(20), MinLength(3)]
        public string City { get; set; }

        [Required, MaxLength(20), MinLength(3)]
        public string Country { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public virtual AppUser User { get; set; }
        public virtual List<OrderProduct> OrderProducts { get; set; }
    }
}