using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Stocks;
using Entities.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class StockGroupManager : IStockGroupService
    {
        private readonly IStockGroupDal _stockGroupDal;
        private readonly IMapper _mapper;
        private readonly BizimNetContext _context;

        public StockGroupManager(IStockGroupDal stockGroupDal, IMapper mapper, BizimNetContext context)
        {
            _stockGroupDal = stockGroupDal;
            _mapper = mapper;
            _context = context;
        }

        public IDataResult<StockGroup> Add(StockGroupAddDto stockGroupAddDto)
        {
            var stockGroup = _mapper.Map<StockGroup>(stockGroupAddDto);
            _stockGroupDal.Add(stockGroup);
            return new SuccessDataResult<StockGroup>(stockGroup, "Ürün grubu başarıyla eklendi.");
        }

        public IResult Delete(int id)
        {
            var stockGroup = _stockGroupDal.Get(s => s.Id == id);
            if (stockGroup == null)
            {
                return new ErrorResult("Ürün grubu bulunamadı.");
            }

            // SAFETY CHECK: Do not delete if products exist in this group
            bool hasStocks = _context.Stocks.Any(s => s.StockGroupId == id);
            if (hasStocks)
            {
                return new ErrorResult("Bu gruba ait ürünler var. Önce ürünleri siliniz veya başka gruba taşıyınız.");
            }

            _stockGroupDal.Delete(stockGroup);
            return new SuccessResult("Ürün grubu silindi.");
        }

        public IDataResult<List<StockGroup>> GetAll()
        {
            // Sorted by SortOrder for the POS UI Tabs
            var list = _stockGroupDal.GetAll().OrderBy(x => x.SortOrder).ToList();
            return new SuccessDataResult<List<StockGroup>>(list);
        }

        public IDataResult<StockGroupAddDto> GetById(int id)
        {
            var stockGroup = _stockGroupDal.Get(s => s.Id == id);
            if (stockGroup == null)
            {
                return new ErrorDataResult<StockGroupAddDto>("Kayıt bulunamadı.");
            }
            var dto = _mapper.Map<StockGroupAddDto>(stockGroup);
            return new SuccessDataResult<StockGroupAddDto>(dto);
        }

        public IResult Update(StockGroup stockGroup)
        {
            _stockGroupDal.Update(stockGroup);
            return new SuccessResult("Ürün grubu güncellendi.");
        }
    }
}