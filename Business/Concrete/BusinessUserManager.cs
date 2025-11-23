using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BusinessUserManager : IBusinessUserService
    {
        private readonly IBusinessUserDal _businessUserDal;
        private readonly ITokenHelper _tokenHelper;
        public BusinessUserManager(IBusinessUserDal businessUserDal, ITokenHelper tokenHelper)
        {
            _businessUserDal = businessUserDal;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<BusinessUser> Add(BusinessUserDto buisnessUser)
        {
            HashingHelper.CreatePasswordHash(buisnessUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new BusinessUser
            {
                Email = buisnessUser.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = buisnessUser.FirstName,
                LastName = buisnessUser.LastName,
                CompanyAddress = buisnessUser.CompanyAddress,
                CompanyName = buisnessUser.CompanyName,
            };
            _businessUserDal.Add(user);
            return new SuccessDataResult<BusinessUser>(user, "Kayıt başarılı.");
        }

        public IDataResult<BusinessUser> ResetPassword(BusinessUserPasswordResetDto businessUser)
        {

            var existingUser = _businessUserDal.Get(x => x.Email == businessUser.Email);
            if (existingUser == null)
                return new ErrorDataResult<BusinessUser>("Kullanıcı bulunamadı.");

            HashingHelper.CreatePasswordHash(businessUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = passwordSalt;

            _businessUserDal.Update(existingUser);

            return new SuccessDataResult<BusinessUser>(existingUser, "Şifre sıfırlandı.");

        }

        public IResult Delete(string id)
        {
            _businessUserDal.Delete(id);
            return new SuccessResult("yay");
        }
        public IDataResult<BusinessUser> UserLogin(BusinessUserLoginDto userForLoginDto)
        {
            var userToCheck = _businessUserDal.Get(x=>x.Email==userForLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<BusinessUser>("");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {

                return new ErrorDataResult<BusinessUser>("Şifre yanlış");
            }

            return new SuccessDataResult<BusinessUser>(userToCheck, "login oldu");
        }

        public IDataResult<List<BusinessUserDetailsDto>> GetAll()
        {
            return new SuccessDataResult<List<BusinessUserDetailsDto>>(_businessUserDal.GetAll());
        }
        public IDataResult<AccessToken> CreateAccessToken(BusinessUser user)
        {
            var accessToken = _tokenHelper.CreateTokenForUser(user);
            return new SuccessDataResult<AccessToken>(accessToken);
        }

        public IDataResult<BusinessUser> GetById(string id)
        {
            var user = _businessUserDal.Get(p => p.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<BusinessUser>("User not found.");
            }
            return new SuccessDataResult<BusinessUser>(user, "User retrieved successfully.");
        }

        public IResult Update(BusinessUser buisnessUser, string id)
        {
            _businessUserDal.Update(buisnessUser);
            return new SuccessResult("yay");
        }


    }
}
