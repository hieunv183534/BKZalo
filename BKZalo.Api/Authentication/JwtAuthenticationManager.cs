using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BKZalo.Api.Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        
        private readonly string key = "This is my test key";
        protected IBaseService<Account> _accountService;
        protected IBaseService<TokenAccount> _tokenAccountService;

        public JwtAuthenticationManager(IBaseService<Account> accountService, IBaseService<TokenAccount> tokenAccountService)
        {
            _accountService = accountService;
            _tokenAccountService = tokenAccountService;
        }

        public string Authenticate(string phoneNumber, string password)
        {
            if (!CheckAccountLogin(phoneNumber,password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, phoneNumber)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            var ra = _tokenAccountService.DeleteByProp("PhoneNumber", phoneNumber);
            var result = _tokenAccountService.Add(new TokenAccount(phoneNumber, $"bearer {tokenHandler.WriteToken(token)}"));
            

            if (result.StatusCode == 201)
            {
                return $"bearer {tokenHandler.WriteToken(token)}";
            }
            else
            {
                return null;
            }
        }

        public Boolean CheckAccountLogin(string phoneNumber, string password)
        {
            var result = _accountService.GetByProp("PhoneNumber", phoneNumber);
            if(result.Response.Data == null)
            {
                return false;
            }
            else
            {
                Account acc = (Account)result.Response.Data;
                if(String.Equals(password, acc.Password))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
