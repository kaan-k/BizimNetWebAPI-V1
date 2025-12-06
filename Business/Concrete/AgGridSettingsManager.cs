using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Customers;
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
        public IDataResult<AgGridSettings> Add(AgGridSettingsDto agGridSetting)
        {
            var agGridDto = _mapper.Map<AgGridSettings>(agGridSetting);
            _agGridSettingsDal.Add(agGridDto);
            return new SuccessDataResult<AgGridSettings>(agGridDto, "Grid ayarı eklendi.");
        }

        public IResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<AgGridSettings>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDataResult<AgGridSettings> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IResult Update(AgGridSettings agGridSetting, string id)
        {
            throw new NotImplementedException();
        }
    }
}
