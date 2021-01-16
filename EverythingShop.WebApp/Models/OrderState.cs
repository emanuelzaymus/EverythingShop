namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// State of <see cref="UserOrder"/>.
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// Completed order, pending for sending.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Sent order waiting for delivery.
        /// </summary>
        Sent = 1,

        /// <summary>
        /// Order delivered.
        /// </summary>
        Delivered = 2
    }
}
