using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Friend : BaseEntity
    {
        public Friend(Guid idA, Guid idB, bool isFriend)
        {
            this.IdA = idA;
            this.IdB = idB;
            this.IsFriend = isFriend;
        }

        public Friend()
        {

        }

        #region Property

        public Guid FriendId { get; set; }

        public Guid IdA { get; set; }

        public Guid IdB { get; set; }

        public bool IsFriend { get; set; }
        #endregion
    }
}
