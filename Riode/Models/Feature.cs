using System.ComponentModel.DataAnnotations;

namespace Riode.Models
{
    public class Feature : BaseEntity
    {
        public string Icon { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
