using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Customer;
using Entities.Concrete.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AgGridSettingsManager : IAgGridSettingsService
    {
        private readonly IMapper _mapper;
        private readonly IAgGridSettingsDal _agGridSettingsDal;
        public AgGridSettingsManager(IAgGridSettingsDal agGridSettingsDal, IMapper mapper) {
            _agGridSettingsDal = agGridSettingsDal;
            _mapper = mapper;
        }
        public IDataResult<Payment> Add(AgGridSettingsDto agGridSetting)
        {
            var agGridDto = _mapper.Map<Payment>(agGridSetting);
            _agGridSettingsDal.Add(agGridDto);
            return new SuccessDataResult<Payment>(agGridDto, "Grid ayarı eklendi.");
        }

        public IResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Payment>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDataResult<Payment> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IResult Update(Payment agGridSetting, string id)
        {
            throw new NotImplementedException();
        }
    }
}
