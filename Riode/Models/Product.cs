using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode.Models
{
    public class Product:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ReviewSum { get; set; } = 0;
        public int? ReviewCount { get; set; } = 0;
        [Required]
        public decimal InitialPrice { get; set; }
        [Range(0,100)]
        public int DiscountPercent { get; set; }
        public string? SKU { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? BrandId{ get; set; }
        public Brand? Brand { get; set; }        
        [Required]
        public int? CategoryId{ get; set; }
        public Category? Category { get; set; }
        [Required]
        public int? MainCategoryId { get; set; }

        public ICollection<ProductColor>? ProductColors { get; set; }
        public ICollection<ProductImage>? ProductImages{ get; set; }
        public ICollection<ProductBadge>? ProductBadges{ get; set; }
        public ICollection<ProductComment>? ProductComments { get; set; }

        [NotMapped]
        public List<int> ColorsIds { get; set; }

        [NotMapped]
        public List<int> BadgesIds { get; set; }

        [NotMapped]
        public IFormFile? MainImage { get; set; }

        [NotMapped]
        public List<IFormFile>? OtherImages { get; set; }

    }
}
