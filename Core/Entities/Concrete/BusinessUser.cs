using Core.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Concrete
{
    public class BusinessUser : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ Changed string -> int

        public string? CompanyName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string? CompanyAddress { get; set; }

        public bool IsAuthorised { get; set; } // ✅ Fixed capitalization (is -> Is)
    }
}