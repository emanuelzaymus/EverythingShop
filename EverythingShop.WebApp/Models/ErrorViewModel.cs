namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// Representation of Error.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// ID of the request.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Whether to show <see cref="RequestId"/>.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
