using AutoMapper;
using Business.Abstract;
using Core.Utilities.Context;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Duty;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DutyManager : IDutyService
    {
        private readonly IMapper _mapper;
        private readonly IDutyDal _dutyDal;
        private readonly IUserContext _user;
        public DutyManager(IDutyDal dutyDal, IMapper mapper, IUserContext userContext) {
            _dutyDal = dutyDal;
            _mapper = mapper;
            _user = userContext;
        }
        public IDataResult<Duty> Add(DutyDto duty)
        {
            var dutyDto = _mapper.Map<Duty>(duty);
            dutyDto.CreatedBy = _user.UserId;
            dutyDto.CreatedAt = DateTime.UtcNow;
            _dutyDal.Add(dutyDto);
            return new SuccessDataResult<Duty>(dutyDto,"Görev başarıyla oluşturuldu.");
        }

        public IResult Delete(string id)
        {
            _dutyDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Duty>> GetAll()
        {
            var duties = _dutyDal.GetAll();

            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<List<Duty>> GetAllByCustomerId(string customerId)
        {
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);

            if(duties == null)
            {
                return new ErrorDataResult<List<Duty>>();
            }

            return new SuccessDataResult<List<Duty>>(duties);

        }

        public IDataResult<List<Duty>> GetAllByStatus(string status)
        {
            var duties = _dutyDal.GetAll(x => x.Status == status);

            if (duties == null)
            {
                return new ErrorDataResult<List<Duty>>();
            }

            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<List<Duty>> GetAllDetails()
        {
            return new SuccessDataResult<List<Duty>>(_dutyDal.GetAllDutyDetails()); 
        }

        public IDataResult<Duty> GetById(string id)
        {
            var dutyToGet = _dutyDal.Get(x=> x.Id == id);

            return new SuccessDataResult<Duty>(dutyToGet);
        }

        public IDataResult<Duty> MarkAsCompleted(string id)
        {
            var duty = _dutyDal.Get(x => x.Id == id);

            if(duty.CompletedBy != null)
            {
                return new ErrorDataResult<Duty>(duty, "Bu görev zaten tamamlanmış.");
            }

            duty.Status = "Tamamlandı";
            duty.UpdatedAt = DateTime.Now;
            duty.CompletedAt = DateTime.Now;
            duty.CompletedBy = _user.UserId;
            _dutyDal.Update(duty);

            return new SuccessDataResult<Duty>(duty,"Görev başarıyla tamamlandı!");

        }

        public IResult Update(Duty request)
        {
            _dutyDal.Update(request);

            return new SuccessResult();
        }

        public IDataResult<Duty> UpdateStatusById(string id, string newStatus)
        {
            var dutyToUpdate = _dutyDal.Get(x=> x.Id == id);
            dutyToUpdate.Status = newStatus;
            _dutyDal.Update(dutyToUpdate);
            return new SuccessDataResult<Duty>(dutyToUpdate);
        }
    }
}
