using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Stocks
{
    public class StockExcelDto
    {
        public string Name { get; set; }
        public string CategoryName { get; set; } // e.g. "İçecekler" -> we will find ID
        public string WarehouseName { get; set; } // e.g. "Merkez Depo" -> we will find ID
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
    }
}
