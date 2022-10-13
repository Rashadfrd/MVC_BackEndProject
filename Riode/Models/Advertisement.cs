namespace Riode.Models
{
    public class Advertisement:BaseEntity
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public bool? Place { get; set; }
    }
}
