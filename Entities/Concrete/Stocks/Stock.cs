using Core.Entities.Abstract;
using Core.Enums;
using Entities.Concrete.Warehouses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Concrete.Stocks
{
    public class Stock : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [MaxLength(50)]
        public string? Barcode { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int Count { get; set; }
        public DeviceType DeviceType { get; set; }

        public int WarehouseId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(WarehouseId))]
        public virtual Warehouse Warehouse { get; set; }
        public int StockGroupId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(StockGroupId))]
        public virtual StockGroup StockGroup { get; set; }
    }
}