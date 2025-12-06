using Core.Entities.Abstract;
using Entities.Concrete.Departments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Employees // ✅ Moved to 'Entities' layer to allow relationship
{
    public class Employee : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ int for SQL

        public string Name { get; set; }
        public string Surname { get; set; }

        // ✅ Fixed Typo (Deparment -> Department) & changed to int
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public string Role { get; set; }
        public string Email { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}