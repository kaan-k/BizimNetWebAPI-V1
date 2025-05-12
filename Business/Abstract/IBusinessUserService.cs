using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBusinessUserService
    {
        IDataResult<BusinessUser> Add(BusinessUserDto businessUser);
        IDataResult<BusinessUser> UserLogin(BusinessUserLoginDto userForLoginDto);
        IResult Update(BusinessUser businessUser, string id);
        IResult Delete(string id);
        IDataResult<AccessToken> CreateAccessToken(BusinessUser user);
        IDataResult<BusinessUser>GetById(string id);
        IDataResult<List<BusinessUser>> GetAll();
    }
}
