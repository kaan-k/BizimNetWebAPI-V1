using Core.Entities.Abstract;
using Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Stocks // ✅ Changed to PLURAL to avoid CS0118 error
{
    public class Stock : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ Changed from string to int for SQL

        public string Name { get; set; }

        // If you uncomment this later, make sure it is an int!
        // public int WarehouseId { get; set; } 

        public int Count { get; set; }

        // EF Core handles Enums automatically (usually stores them as int in the DB)
        public DeviceType DeviceType { get; set; }
    }
}