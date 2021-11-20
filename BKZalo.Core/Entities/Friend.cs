using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Friend
    {
        #region Property

        public Guid FriendId { get; set; }

        public Guid IdA { get; set; }

        public Guid IdB { get; set; }

        public bool IsFriend { get; set; }
        #endregion
    }
}
