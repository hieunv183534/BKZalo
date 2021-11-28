using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Block : BaseEntity
    {
        public Guid BlockId { get; set; }

        public Guid BlockFromId { get; set; }

        public Guid BlockToId { get; set; }
    }
}
