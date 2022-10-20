using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode.Models
{
    public class Slider : BaseEntity
    {
        public string? ImageUrl { get; set; }

        [Required]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }

        [Required]
        public int Order { get; set; }
        public bool? Place { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
