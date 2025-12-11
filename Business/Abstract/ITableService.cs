using Core.Utilities.Results;
using Entities.Concrete.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITableService
    {
        IDataResult<Table> Add(TableAddDto tableAddDto);
        IResult Update(Table table);
        IResult Delete(int id);
        IDataResult<TableAddDto> GetById(int id);
        IDataResult<List<Table>> GetAll();
        public IResult MassAdd(TableMassAddDto dto);
        IDataResult<List<TableDetailDto>> GetBySection(int sectionId);
    }
}
