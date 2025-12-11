using Core.Utilities.Results;
using Entities.Concrete.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISectionService
    {
        IDataResult<Section> Add(SectionAddDto sectionAddDto);
        IResult Update(Section section);
        IResult Delete(int id);
        IDataResult<SectionAddDto> GetById(int id);
        IDataResult<List<Section>> GetAll();
    }
}
