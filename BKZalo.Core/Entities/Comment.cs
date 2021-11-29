using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Comment : BaseEntity
    {

        public Comment()
        {

        }

        #region Property

        public Guid CommentId { get; set; }

        public Guid AccountId { get; set; }

        public string Content { get; set; }

        public Guid PostId { get; set; }

        public Account Poster { get; set; }

        #endregion
    }
}
