using EverythingShop.WebApp.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// Order of <see cref="AppUser"/>.
    /// </summary>
    public class UserOrder
    {
        /// <summary>
        /// Order ID.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// ID of <see cref="AppUser"/> that this <see cref="UserOrder"/> belongs to.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Address contact name.
        /// </summary>
        [MaxLength(50), MinLength(3)]
        public string ContactName { get; set; }

        /// <summary>
        /// Address street with street number.
        /// </summary>
        [MaxLength(50), MinLength(3)]
        public string StreetAddress { get; set; }

        /// <summary>
        /// Address postal code.
        /// </summary>
        [MaxLength(10), MinLength(3), DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Address city.
        /// </summary>
        [MaxLength(20), MinLength(3)]
        public string City { get; set; }

        /// <summary>
        /// Address county.
        /// </summary>
        [MaxLength(20), MinLength(3)]
        public string Country { get; set; }

        /// <summary>
        /// When was this order completed by user.
        /// </summary>
        [Display(Name = "Ordered On")]
        [DataType(DataType.DateTime)]
        public DateTime? OrderedOn { get; set; }

        /// <summary>
        /// State of the order. For example: <see cref="OrderState.Pending"/>, <see cref="OrderState.Sent"/> 
        /// or <see cref="OrderState.Delivered"/>. If it is <c>null</c> then this order was not completed.
        /// </summary>
        public OrderState? State { get; set; }

        /// <summary>
        /// Owner of this order.
        /// </summary>
        public virtual AppUser User { get; set; }

        /// <summary>
        /// Products in this order.
        /// </summary>
        public virtual List<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Total Price of this order.
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalPrice() => OrderProducts.Sum(op => op.Quantity * op.Product.Price);
    }
}
