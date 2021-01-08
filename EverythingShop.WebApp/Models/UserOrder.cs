using EverythingShop.WebApp.Areas.Identity.Data;
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

        [MaxLength(50), MinLength(3)]
        public string ContactName { get; set; }

        [MaxLength(50), MinLength(3)]
        public string StreetAddress { get; set; }

        [MaxLength(10), MinLength(3), DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [MaxLength(20), MinLength(3)]
        public string City { get; set; }

        [MaxLength(20), MinLength(3)]
        public string Country { get; set; }

        [Display(Name = "Ordered On")]
        [DataType(DataType.DateTime)]
        public DateTime? OrderedOn { get; set; }

        public OrderState? State { get; set; }

        public virtual AppUser User { get; set; }
        public virtual List<OrderProduct> OrderProducts { get; set; }

        public decimal TotalPrice => OrderProducts.Sum(p => p.Quantity * p.Product.Price);
    }
}
