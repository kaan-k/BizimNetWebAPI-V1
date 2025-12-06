using Core.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Settings
{
    public class AgGridSettings : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ Changed from string to int

        public int UserId { get; set; } // ✅ Changed from string to int for SQL

        // Future Step: Once you fix your User entity, add this:
        // [ForeignKey("UserId")]
        // public virtual User User { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}