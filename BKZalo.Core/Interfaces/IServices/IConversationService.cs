using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IServices
{
    public interface IConversationService : IBaseService<Conversation>
    {
        ServiceResult GetListConversation(Guid userId, int index, int count);

        ServiceResult GetConversation(Guid userId,Guid conversationId, int index, int count);
    }
}
