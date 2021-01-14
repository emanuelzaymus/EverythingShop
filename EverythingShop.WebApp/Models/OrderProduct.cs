using System.ComponentModel.DataAnnotations;

namespace EverythingShop.WebApp.Models
{
    public class OrderProduct
    {
        [Key]
        public int UserOrderId { get; set; }

        [/*Key,*/ Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        public virtual UserOrder UserOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
