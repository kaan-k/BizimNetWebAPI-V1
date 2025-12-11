using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TableManager:ITableService
    {
        private readonly IMapper _mapper;
        private readonly ITableDal _tableDal;
        private readonly BizimNetContext _context; // Direct DB access for complex checks
        private readonly IOrderDal _orderDal;

        public TableManager(IMapper mapper, ITableDal tableDal, BizimNetContext context, IOrderDal orderDal)
        {
            _mapper = mapper;
            _tableDal = tableDal;
            _context = context;
            _orderDal = orderDal;
        }

        public IDataResult<Table> Add(TableAddDto tableAddDto)
        {
            var table = _mapper.Map<Table>(tableAddDto);

            // Logic: Default new tables to 'Free'
            table.CurrentStatus = TableStatus.Free;

            _tableDal.Add(table);
            return new SuccessDataResult<Table>(table, "Masa başarıyla eklendi.");
        }
        public IResult MassAdd(TableMassAddDto dto)
        {
            if (dto.Count <= 0) return new ErrorResult("Eklenecek masa sayısı 0'dan büyük olmalıdır.");

            var newTables = new List<Table>();

            for (int i = 0; i < dto.Count; i++)
            {
                int currentNum = dto.StartNumber + i;

                // Format: "S-1", "S-2"
                string tableName = $"{dto.Prefix}{currentNum}";

                newTables.Add(new Table
                {
                    Name = tableName,
                    SectionId = dto.SectionId,
                    CurrentStatus = Entities.Concrete.Tables.TableStatus.Free,
                    SortOrder = currentNum // Auto-sort by number
                });
            }

            foreach (var table in newTables)
            {
                _tableDal.Add(table);
            }

            return new SuccessResult($"{dto.Count} adet masa başarıyla oluşturuldu.");
        }

        public IResult Delete(int id)
        {
            var table = _tableDal.Get(t => t.Id == id);

            if (table == null)
            {
                return new ErrorResult("Masa bulunamadı.");
            }

            // --- BUSINESS LOGIC (Like your Stock check) ---
            // Prevent deleting a table if it is currently occupied
            if (table.CurrentStatus != TableStatus.Free)
            {
                return new ErrorResult("Hareketli Masa: Masa şu an dolu veya rezerve. Silinemez!");
            }

            // Optional: If you have an Orders table later, check that too:
            // bool hasActiveOrders = _context.Orders.Any(o => o.TableId == id && o.Status == OrderStatus.Open);
            // if (hasActiveOrders) return new ErrorResult("Açık sipariş var, silinemez!");

            _tableDal.Delete(table);
            return new SuccessResult("Masa başarıyla silindi.");
        }

        public IDataResult<List<Table>> GetAll()
        {
            // Sort by the SortOrder we added so the Grid looks correct
            var tables = _tableDal.GetAll().OrderBy(t => t.SortOrder).ToList();
            return new SuccessDataResult<List<Table>>(tables);
        }

        public IDataResult<List<TableDetailDto>> GetBySection(int sectionId)
        {
            // 1. Get Tables for this Section
            var tables = _tableDal.GetAll(t => t.SectionId == sectionId)
                                  .OrderBy(t => t.SortOrder)
                                  .ToList();

            // 2. Get Active (Unpaid) Orders for these tables
            // Optimization: We only fetch orders that belong to the tables we just grabbed
            var tableIds = tables.Select(t => t.Id).ToList();
            var activeOrders = _orderDal.GetAll(o => tableIds.Contains(o.TableId) && !o.IsPaid);

            // 3. Map Table + Order Total into DTO
            var result = tables.Select(t => new TableDetailDto
            {
                Id = t.Id,
                Name = t.Name,
                SectionId = t.SectionId,
                CurrentStatus = t.CurrentStatus,
                SortOrder = t.SortOrder,

                // Look for an order for this table. If found, use TotalAmount. Else 0.
                CurrentBillAmount = activeOrders.FirstOrDefault(o => o.TableId == t.Id)?.TotalAmount ?? 0
            }).ToList();

            return new SuccessDataResult<List<TableDetailDto>>(result);
        }

        public IDataResult<TableAddDto> GetById(int id)
        {
            var table = _tableDal.Get(t => t.Id == id);
            if (table == null)
            {
                return new ErrorDataResult<TableAddDto>("Masa bulunamadı.");
            }

            var dto = _mapper.Map<TableAddDto>(table);
            return new SuccessDataResult<TableAddDto>(dto);
        }

        public IResult Update(Table table)
        {
            // You might want to validate SectionId exists here
            _tableDal.Update(table);
            return new SuccessResult("Masa güncellendi.");
        }
    }
}
