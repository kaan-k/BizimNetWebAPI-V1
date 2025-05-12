using Core.Entities;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class EmployeeJwtHelper : IEmployeeTokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _TokenOptions;
        private DateTime _accessTokenExpiration;
        public EmployeeJwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _TokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }
        public EmployeeAccessToken CreateToken(Employee employee)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_TokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_TokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_TokenOptions, employee, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new EmployeeAccessToken
            {
                EmployeeId=employee.Id,
                Token = token,
                Expiration = _accessTokenExpiration.ToString("yyy-MM-dd HH:mm:ss")
            };
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, Employee employee,
            SigningCredentials signingCredentials)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(employee),
                signingCredentials: signingCredentials
                );
            return jwt;
        }
        private IEnumerable<Claim> SetClaims(Employee employee)
        {
            var claims = new List<Claim>();

            claims.AddNameIdentifier(employee.Id);
            claims.AddName($"{employee.Name} {employee.Surname}");
         
            return claims;
        }

    }
}
