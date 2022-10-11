namespace Riode.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? IsModified { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
