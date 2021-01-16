using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// Represents relation between <see cref="EverythingShop.WebApp.Models.UserOrder"/> 
    /// and <see cref="EverythingShop.WebApp.Models.Product"/>. M:N relation table.
    /// </summary>
    public class OrderProduct
    {
        /// <summary>
        /// PFK ID of <see cref="UserOrder"/>.
        /// </summary>
        [Key]
        public int UserOrderId { get; set; }

        /// <summary>
        /// PFK ID of <see cref="Product"/>.
        /// </summary>
        [/*Key,*/ Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Ördered produc quantoty of <see cref="Product"/>.
        /// </summary>
        [Required]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// User order of <see cref="Product"/>.
        /// </summary>
        public virtual UserOrder UserOrder { get; set; }

        /// <summary>
        /// Product of <see cref="UserOrder"/>.
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
