namespace Riode.Models
{
    public class ProductBadge
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Badge Badge { get; set; }
        public int BadgeId { get; set; }
    }
}
