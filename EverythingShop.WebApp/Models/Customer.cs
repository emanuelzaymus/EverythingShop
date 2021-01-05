using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        [Required, MaxLength(50), MinLength(5), EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string EncryptedPassword { get; set; }

        public virtual List<UserOrder> CustomerOrders { get; set; }
    }
}
