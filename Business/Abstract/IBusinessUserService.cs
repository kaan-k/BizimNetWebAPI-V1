using Core.Entities.Concrete; // Where BusinessUser entity is
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IBusinessUserService
    {
        IDataResult<BusinessUser> Add(BusinessUserDto businessUser);
        IDataResult<BusinessUser> ResetPassword(BusinessUserPasswordResetDto businessUser);

        IDataResult<BusinessUser> UserLogin(BusinessUserLoginDto userForLoginDto);

        // ✅ Changed string -> int
        IResult Update(BusinessUser businessUser, int id);
        IResult Delete(int id);

        IDataResult<AccessToken> CreateAccessToken(BusinessUser user);

        // ✅ Changed string -> int
        IDataResult<BusinessUser> GetById(int id);

        IDataResult<List<BusinessUser>> GetAll();
    }
}