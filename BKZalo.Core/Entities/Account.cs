using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Account : BaseEntity
    {
        #region Property

        public Guid AccountId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string AvatarUrl { get; set; }

        #endregion
    }
}
