using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IRepositories
{
    public interface IConversationRepository
    {
        List<Conversation> GetListConversation(Guid userId, int index, int count);

        Message GetLastMessage(Guid conversationId);

        List<Message> GetMessages(Guid conversationId, int index, int count); 
    }
}
