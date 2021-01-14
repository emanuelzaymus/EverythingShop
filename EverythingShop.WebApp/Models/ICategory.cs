namespace EverythingShop.WebApp.Models
{
    public interface ICategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsMainCategory => this is MainCategory;
    }
}
