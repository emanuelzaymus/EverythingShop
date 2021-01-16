namespace EverythingShop.WebApp.Models
{
    /// <summary>
    /// Category interface.
    /// </summary>
    public interface ICategory
    {
        /// <summary>
        /// ID of the Category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the Category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <c>True</c> if Category is <see cref="MainCategory"/>. <c>False</c> if it's <see cref="SubCategory"/>.
        /// </summary>
        public bool IsMainCategory => this is MainCategory;
    }
}
