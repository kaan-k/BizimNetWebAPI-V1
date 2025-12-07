using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class WarehouseManager : IWarehouseService
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseDal _warehouseDal;
        private readonly BizimNetContext _context;
        public WarehouseManager(IMapper mapper, IWarehouseDal warehouseDal, BizimNetContext context) {
        
            _mapper = mapper;
            _warehouseDal = warehouseDal;
            _context = context;
        
        }
        public IDataResult<Warehouse> Add(WarehouseAddDto warehouseAddDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseAddDto);
            _warehouseDal.Add(warehouse);
            return new SuccessDataResult<Warehouse>(warehouse);
        }

        public IResult Delete(int id)
        {
            var warehouse = _warehouseDal.Get(w=>w.Id == id);

            if (warehouse == null)
            {
                return new ErrorResult("Depo bulunamadı");
            }

            bool hasStocks  =_context.Stocks.Any(b => b.WarehouseId == id);
            if (hasStocks)
            {
                return new ErrorResult("Hareketli Depo: Depoya kayıtlı stoklar var. Silinemez!");
            }

            _warehouseDal.Delete(warehouse);

            return new SuccessResult("Depo başarıyla silindi.");
        }

        public IDataResult<List<Warehouse>> GetAll()
        {
            var warehouses = _warehouseDal.GetAllWithStocks();
            return new SuccessDataResult<List<Warehouse>>(warehouses);
        }
        

        public IDataResult<WarehouseAddDto> GetById(int id)
        {
            var warehouse = _warehouseDal.GetByIdWithStocks(id);
            if (warehouse == null)
            {
                return new ErrorDataResult<WarehouseAddDto>("Depo bulunamadı.");
            }
            var dto = _mapper.Map<WarehouseAddDto>(warehouse);

            return new SuccessDataResult<WarehouseAddDto>(dto);
        }

        public IResult Update(Warehouse warehouse)
        {
            _warehouseDal.Update(warehouse);
            return new SuccessResult("Depo güncellendi.");
        }
    }
}
