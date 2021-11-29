using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Post : BaseEntity
    {
        public Post()
        {

        }

        #region Property

        public Guid PostId { get; set; }

        [Requied]
        public Guid AccountId { get; set; }

        public string Described { get; set; }

        public List<string> MediaUrls { get; set; }

        public string AllMediaUrl { get; set; }

        public List<Guid> AccountIdLikeds { get; set; }

        public string AllAccountIdLiked { get; set; }

        public int Like { get; set; }

        public int Comment { get; set; }

        public Account Author { get; set; }

        public bool CanEdit { get; set; }

        public bool CanComment { get; set; }

        public bool IsLiked { get; set; }

        #endregion
    }
}
