namespace Riode.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReviewSum { get; set; }
        public int ReviewCount { get; set; }
        public decimal InitialPrice { get; set; }
        public int? DiscountPrice { get; set; }
        public string SKU { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductImage> ProductImages{ get; set; }
        public ICollection<Brand> Brands{ get; set; }
        public ICollection<Category> Categories{ get; set; }
        public ICollection<ProductBadge> ProductBadges{ get; set; }


    }
}
