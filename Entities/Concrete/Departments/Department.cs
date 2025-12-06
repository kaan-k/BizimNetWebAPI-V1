using Core.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.Departments
{
    public class Department : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        // ✅ Changed from string to int
        // Note: Once you fix your "Person" or "User" class, 
        // you should come back and add a Navigation Property here.
        public int ManagerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Use UtcNow
        public DateTime? UpdatedAt { get; set; }
    }
}