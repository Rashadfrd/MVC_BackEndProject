using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode.Models
{
    public class Advertisement:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public bool? Place { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
