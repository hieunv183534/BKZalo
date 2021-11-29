using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Message : BaseEntity
    {
        public Message()
        {

        }

        public Guid MessageId { get; set; }

        public Guid SenderId { get; set; }

        public Account Sender { get; set; }

        public Guid ReceiverId { get; set; }

        public Account Receiver { get; set; }

        public Guid ConversationId { get; set; }

        public string Content { get; set; }
    }
}
