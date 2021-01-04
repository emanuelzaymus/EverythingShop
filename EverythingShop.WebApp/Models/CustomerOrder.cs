using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class CustomerOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required, StringLength(50)]
        public string ContactName { get; set; }

        [Required, StringLength(50)]
        public string StreetAddress { get; set; }

        [Required, StringLength(10), DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required, StringLength(20)]
        public string City { get; set; }

        [Required, StringLength(20)]
        public string Country { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual List<OrderProduct> OrderProducts { get; set; }
    }
}
