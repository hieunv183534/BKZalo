using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class TokenAccount : BaseEntity
    {

        public TokenAccount(string phoneNumber, string token)
        {
            this.PhoneNumber = phoneNumber;
            this.Token = token;
        }

        public TokenAccount()
        {

        }

        #region Property

        public string PhoneNumber { get; set; }

        public string Token { get; set; }
        #endregion
    }
}
