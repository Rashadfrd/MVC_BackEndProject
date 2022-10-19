namespace Riode.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsMain { get; set; }
        public ICollection<Category>? SubCategory { get; set; }
        public int? MainCategoryId { get; set; }
        public Category? MainCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
