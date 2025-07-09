using AutoMapper;
using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class StockManager : IStockService
    {
        private readonly IStockDal _stockDal;
        private readonly IMapper _mapper;
        public StockManager(IStockDal stockDal, IMapper mapper) {
            _stockDal = stockDal;
            _mapper = mapper;
        }
        public IDataResult<StockAddDto> Add(StockAddDto stock)
        {
            var mappedResult = _mapper.Map<Stock>(stock);
            _stockDal.Add(mappedResult);
            return new SuccessDataResult<StockAddDto>(stock);
        }

        public IResult Delete(string id)
        {
            var stockToDelete = _stockDal.Get(x=>x.Id  == id);
            if (stockToDelete != null)
            {
                return new ErrorResult();
            }
            _stockDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Stock>> GetByDeviceType(DeviceType devicetype)
        {
            var result = _stockDal.GetAll(x=>x.DeviceType == devicetype);
            if (result == null)
            {
                return new ErrorDataResult<List<Stock>>(result,"err");
            }
            return new SuccessDataResult<List<Stock>>(result);
        }

        public IDataResult<Stock> Update(Stock servicing)
        {
            throw new NotImplementedException();
        }
    }
}
