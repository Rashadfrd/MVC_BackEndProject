using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode.Models
{
    public class Brand : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Product>? Products { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
