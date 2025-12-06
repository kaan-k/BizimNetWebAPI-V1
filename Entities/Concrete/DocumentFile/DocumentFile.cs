using Core.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete.Customers;
namespace Entities.Concrete.DocumentFile
{

    public class DocumentFile : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ Changed to int

        public int? OfferId { get; set; } // ✅ Changed to int? (Nullable)

        // ✅ Customer Relationship
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; } // Navigation property

        public int? PersonId { get; set; }
        public int? DepartmentId { get; set; }

        // ⚠️ SQL cannot store lists directly.
        // I marked this [NotMapped] so your project compiles.
        // ideally, you should create a separate table "FileDownloads" to track this.
        [NotMapped]
        public List<int> DownloaderIds { get; set; } = new List<int>();

        public string DocumentName { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentFullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Use UtcNow
        public DateTime? LastModifiedAt { get; set; }

        public string? DocumentType { get; set; }
    }

}
