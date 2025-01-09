using System.ComponentModel.DataAnnotations;

namespace KuzinShop.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
