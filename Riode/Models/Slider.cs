namespace Riode.Models
{
    public class Slider : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool? Place { get; set; }

    }
}
