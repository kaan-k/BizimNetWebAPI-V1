using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Customer;
using Entities.Concrete.InviteToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class InviteTokenManager : IInviteTokenService
    {
        private readonly IMapper _mapper;
        private readonly IInviteTokenDal _inviteTokenDal;
        public InviteTokenManager(IInviteTokenDal inviteTokenDal, IMapper mapper)
        {
            _inviteTokenDal = inviteTokenDal;
            _mapper = mapper;
        }
        public IResult Add(InviteTokenCreateDto token)
        {
            var tokenDto = _mapper.Map<InviteToken>(token);

            tokenDto.Token = Guid.NewGuid().ToString();
            tokenDto.Used = false;
            _inviteTokenDal.Add(tokenDto);

            return new SuccessResult("Invite token eklendi.");

        }

        public IResult Validate(InviteTokenValidationDto token)
        {
            var tokenToCheck = _inviteTokenDal.Get(x => x.Token == token.Token);

            if(tokenToCheck == null)
            {
                return new ErrorResult("Token bulunamadı.");
            }
            if (tokenToCheck.Used)
            {
                return new ErrorResult("Token zaten kullanılmış.");
            }
            if(token.Email != tokenToCheck.Email)
            {
                return new ErrorResult("Geçersiz token. Email uyuşmuyor.");
            }
            if(token.Expiration > tokenToCheck.Expiration)
            {
                return new ErrorResult("Geçersiz token. Süresi dolmuş.");
            }
            tokenToCheck.Used = true;
            _inviteTokenDal.Update(tokenToCheck);
            return new SuccessResult();
        }
    }
}
