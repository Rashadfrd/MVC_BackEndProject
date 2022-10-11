namespace Riode.Models
{
    public class Badge:BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public ICollection<ProductBadge> ProductBadges { get; set; }
    }
}
