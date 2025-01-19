using Microsoft.AspNetCore.Identity;

namespace KuzinShop.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } // Флаг блокировки
    }
}
