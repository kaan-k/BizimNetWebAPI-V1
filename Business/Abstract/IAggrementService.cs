
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete.Aggrements;

namespace Business.Abstract
{
    public interface IAggrementService
    {
        IDataResult<Aggrement> Add(AggrementDto businessUser);
        IResult Update(Aggrement businessUser, string id);
        IResult Delete(string id);
        IDataResult<Aggrement> GetById(string id);
        IDataResult<List<Aggrement>> GetAll();
    }
}
