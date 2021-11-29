using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Conversation : BaseEntity
    {
        public Conversation()
        {

        }

        public Guid ConversationId { get; set; }

        public string AllMemberId { get; set; }

        public string AllReadId { get; set; }

        public Account Partner { get; set; }

        public Message LastMessage { get; set; }
    }
}
