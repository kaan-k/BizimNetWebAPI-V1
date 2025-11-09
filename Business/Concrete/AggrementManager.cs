using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AggrementManager : IAggrementService
    {
        private readonly IMapper _mapper;
        private readonly IAggrementDal _aggrementDal;
        public AggrementManager(IMapper mapper, IAggrementDal aggrementDal) {
            _mapper = mapper;
            _aggrementDal = aggrementDal;
        }
        public IDataResult<Aggrement> Add(AggrementDto aggrement)
        {
            var aggrementToAdd = _mapper.Map<Aggrement>(aggrement);
            _aggrementDal.Add(aggrementToAdd);
            return new SuccessDataResult<Aggrement>(aggrementToAdd);

        }

        public IResult Delete(string id)
        {
            _aggrementDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Aggrement>> GetAll()
        {
            var aggrements = _aggrementDal.GetAll();
            return new SuccessDataResult<List<Aggrement>>(aggrements);
        }

        public IDataResult<Aggrement> GetById(string id)
        {
            var aggrement = _aggrementDal.Get(x => x.Id == id);
            return new SuccessDataResult<Aggrement>(aggrement);
        }

        public IResult Update(Aggrement aggrement, string id)
        {
            throw new NotImplementedException();
        }
    }
}
