using System.ComponentModel.DataAnnotations;

namespace Riode.ViewModels
{
    public class UserRegistrationVM
    {
        [Required,StringLength(30)]
        public string Name{ get; set; }

        [Required,StringLength(50)]
        public string Surname{ get; set; }

        [Required,DataType(DataType.EmailAddress)]
        public string Email{ get; set; }

        [Required, StringLength(40),DataType(DataType.Password)]
        public string Password{ get; set; }

        [Required,StringLength(40),DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword{ get; set; }
        public bool isRemember { get; set; } = false;
    }
}
