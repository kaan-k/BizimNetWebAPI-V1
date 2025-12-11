using AutoMapper;
using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Stocks; // ✅ Plural Namespace
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml; // EPPlus namespace
using Microsoft.AspNetCore.Http;
namespace Business.Concrete
{
    public class StockManager : IStockService
    {
        private readonly IStockDal _stockDal;
        private readonly IMapper _mapper;
        private readonly IStockGroupDal _stockGroupDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly BizimNetContext _context;
        public StockManager(IStockDal stockDal, IMapper mapper, BizimNetContext context, IStockGroupDal stockGroupDal, IWarehouseDal warehouseDal)
        {
            _stockDal = stockDal;
            _mapper = mapper;
            _context = context;
            _stockGroupDal = stockGroupDal;
            _warehouseDal = warehouseDal;
        }

        public IDataResult<StockAddDto> Add(StockAddDto stockDto)
        {
            // 1. SAFETY CHECK: Does this StockGroup actually exist?
            var groupExists = _stockGroupDal.Get(g => g.Id == stockDto.StockGroupId);
            if (groupExists == null)
            {
                return new ErrorDataResult<StockAddDto>("Seçilen Ürün Grubu (Kategori) bulunamadı. Lütfen önce kategori ekleyin.");
            }

            // 2. SAFETY CHECK: Does this Warehouse exist?
            //var warehouseExists = _warehouseDal.Get(w => w.Id == stockDto.WarehouseId);
            //if (warehouseExists == null)
            //{
            //    return new ErrorDataResult<StockAddDto>("Seçilen Depo bulunamadı.");
            //}

            // 3. Map and Save
            var stock = _mapper.Map<Stock>(stockDto);

            // Ensure relationships are set cleanly (EF Core sometimes gets confused if you pass full objects)
            //stock.StockGroup = null;
            stock.Warehouse = null;
            stock.IsActive = true;
            _stockDal.Add(stock);

            return new SuccessDataResult<StockAddDto>(stockDto,"Stok başarıyla eklendi.");
        }

        public IResult Delete(int id)
        {
            // 1. Check if it exists
            var stockToDelete = _stockDal.Get(x => x.Id == id);

            // 2. Fix: Only return error if it is NULL (not found)
            if (stockToDelete == null)
            {
                return new ErrorResult("Stok bulunamadı.");
            }

            // 3. Delete
            _stockDal.Delete(id);
            return new SuccessResult("Stok silindi.");
        }

        public IDataResult<List<Stock>> GetAll()
        {
            var result = _stockDal.GetAll();
            return new SuccessDataResult<List<Stock>>(result, "Stok listesi getirildi.");
        }

        public IDataResult<List<Stock>> GetByDeviceType(DeviceType deviceType)
        {
            // Note: Ensure DeviceType matches exactly how it's stored in SQL (int or string)
            var result = _stockDal.GetAll(x => x.DeviceType == deviceType);

            if (result == null || !result.Any())
            {
                return new ErrorDataResult<List<Stock>>(result, "Bu tipte stok bulunamadı.");
            }
            return new SuccessDataResult<List<Stock>>(result);
        }

        public IDataResult<Stock> Update(Stock stock)
        {
            // 1. Check existence
            var existingStock = _stockDal.Get(x => x.Id == stock.Id);
            if (existingStock == null)
            {
                return new ErrorDataResult<Stock>(null, "Güncellenecek stok bulunamadı.");
            }

            // 2. Update fields
            existingStock.Name = stock.Name;
            existingStock.Count = stock.Count;
            existingStock.DeviceType = stock.DeviceType;
            // existingStock.WarehouseId = stock.WarehouseId; // Uncomment if you use this

            // 3. Save
            _stockDal.Update(existingStock);

            return new SuccessDataResult<Stock>(existingStock, "Stok güncellendi.");
        }

        public IResult ImportExcel(IFormFile file)
        {
            // 1. License (EPPlus 8+)
            ExcelPackage.License.SetNonCommercialPersonal("MyProject");

            if (file == null || file.Length == 0)
                return new ErrorResult("Dosya boş.");

            var newStocks = new List<Stock>();
            var stockGroups = _stockGroupDal.GetAll();
            var warehouses = _warehouseDal.GetAll();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var sheet = package.Workbook.Worksheets[0];
                    if (sheet.Dimension == null) return new ErrorResult("Excel dosyası boş.");

                    var rowCount = sheet.Dimension.Rows;
                    var colCount = sheet.Dimension.Columns;

                    // --- STEP A: FIND HEADERS DYNAMICALLY ---
                    // We loop through Row 1 to find which Column Index belongs to "Stok Adı", "Kategori", etc.

                    var headers = new Dictionary<string, int>();

                    for (int col = 1; col <= colCount; col++)
                    {
                        var headerVal = sheet.Cells[1, col].Text?.Trim().ToLower(); // Normalize to lowercase
                        if (!string.IsNullOrEmpty(headerVal))
                        {
                            // Map known headers (you can add variations like "stock name" or "ürün adı")
                            if (headerVal == "stok adı" || headerVal == "ürün adı" || headerVal == "name") headers["name"] = col;
                            if (headerVal == "kategori" || headerVal == "category" || headerVal == "grup") headers["group"] = col;
                            if (headerVal == "depo" || headerVal == "warehouse") headers["warehouse"] = col;
                            if (headerVal == "miktar" || headerVal == "count" || headerVal == "adet") headers["count"] = col;
                            if (headerVal == "fiyat" || headerVal == "price") headers["price"] = col;
                            if (headerVal == "barkod" || headerVal == "barcode") headers["barcode"] = col;
                        }
                    }

                    // Validation: Did we find the required columns?
                    if (!headers.ContainsKey("name")) return new ErrorResult("Excel'de 'Stok Adı' veya 'Ürün Adı' başlığı bulunamadı.");
                    if (!headers.ContainsKey("group")) return new ErrorResult("Excel'de 'Kategori' başlığı bulunamadı.");
                    if (!headers.ContainsKey("warehouse")) return new ErrorResult("Excel'de 'Depo' başlığı bulunamadı.");

                    // --- STEP B: READ DATA USING DYNAMIC INDEXES ---

                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Use the map to get the correct column index
                        var rowName = sheet.Cells[row, headers["name"]].Text?.Trim();
                        if (string.IsNullOrEmpty(rowName)) continue; // Skip empty rows

                        var categoryName = sheet.Cells[row, headers["group"]].Text?.Trim();
                        var warehouseName = sheet.Cells[row, headers["warehouse"]].Text?.Trim();

                        // Safe lookup for optionals
                        var countText = headers.ContainsKey("count") ? sheet.Cells[row, headers["count"]].Text : "0";
                        var priceText = headers.ContainsKey("price") ? sheet.Cells[row, headers["price"]].Text : "0";
                        var barcode = headers.ContainsKey("barcode") ? sheet.Cells[row, headers["barcode"]].Text?.Trim() : null;

                        // --- LOGIC REMAINS THE SAME ---
                        var group = stockGroups.FirstOrDefault(g => g.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
                        var warehouse = warehouses.FirstOrDefault(w => w.Name.Equals(warehouseName, StringComparison.OrdinalIgnoreCase));

                        if (group == null) return new ErrorResult($"Satır {row}: '{categoryName}' kategorisi bulunamadı. Lütfen sisteme ekleyin.");
                        if (warehouse == null) return new ErrorResult($"Satır {row}: '{warehouseName}' deposu bulunamadı.");

                        var stock = new Stock
                        {
                            Name = rowName,
                            StockGroupId = group.Id,
                            WarehouseId = warehouse.Id,
                            Count = int.TryParse(countText, out int c) ? c : 0,
                            UnitPrice = decimal.TryParse(priceText, out decimal p) ? p : 0,
                            Barcode = barcode,
                            DeviceType = Core.Enums.DeviceType.Client,
                            IsActive = true
                        };

                        newStocks.Add(stock);
                    }
                }
            }

            foreach (var stock in newStocks)
            {
                _stockDal.Add(stock);
            }

            return new SuccessResult($"{newStocks.Count} adet stok eklendi.");
        }
    }
}