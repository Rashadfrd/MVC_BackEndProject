namespace Riode.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
