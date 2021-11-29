using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IServices;
using BKZalo.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKZalo.Api.Authentication
{
    public class ValidateTokenClass
    {
        public static bool ValidateToken(string token)
        {
            if (token.StartsWith("Bearer"))
            {
                token = token.Replace("Bearer", "bearer");
            }
            BaseRepository<TokenAccount> tokenAccountRepo = new BaseRepository<TokenAccount>();
            var tokenAccount = tokenAccountRepo.GetByProp("Token", token);
            if(tokenAccount != null)
            {
                return true;
            }
            return false;
        }
    }
}
