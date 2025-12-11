using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Concrete.Stocks
{
    public class StockGroup : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // "Beverages", "Desserts"
        //public string ColorCode { get; set; } = "#3b82f6"; // Hex color for the UI Button

        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } // To put "Drinks" before "Food"

        [JsonIgnore]
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
