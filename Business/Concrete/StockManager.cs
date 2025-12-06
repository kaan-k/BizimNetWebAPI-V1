using AutoMapper;
using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Stocks; // ✅ Plural Namespace
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class StockManager : IStockService
    {
        private readonly IStockDal _stockDal;
        private readonly IMapper _mapper;
        private readonly BizimNetContext _context;
        public StockManager(IStockDal stockDal, IMapper mapper, BizimNetContext context)
        {
            _stockDal = stockDal;
            _mapper = mapper;
            _context = context;
        }

        public IDataResult<StockAddDto> Add(StockAddDto stockDto)
        {
            var stockEntity = _mapper.Map<Stock>(stockDto);
            // SQL generates ID automatically
            _stockDal.Add(stockEntity);

            _context.SaveChanges();

            return new SuccessDataResult<StockAddDto>(stockDto, "Stok eklendi.");
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
    }
}