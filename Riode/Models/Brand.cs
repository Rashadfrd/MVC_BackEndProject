namespace Riode.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
