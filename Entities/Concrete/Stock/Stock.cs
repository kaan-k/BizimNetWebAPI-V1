using Core.Entities.Abstract;
using Core.Enums;
using Entities.Concrete.Warehouses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Concrete.Stocks // ✅ Changed to PLURAL to avoid CS0118 error
{
    public class Stock : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
        public DeviceType DeviceType { get; set; }

        // --- Relationships ---
        public int WarehouseId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }
    }
}