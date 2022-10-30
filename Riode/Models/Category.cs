using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public bool? IsMain { get; set; }
        public ICollection<Category>? SubCategory { get; set; }
        public int? MainCategoryId { get; set; }
        public Category? MainCategory { get; set; }
        public ICollection<Product>? Products { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
