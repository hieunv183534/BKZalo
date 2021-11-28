using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IServices
{
    public interface IFriendService : IBaseService<Friend>
    {
        ServiceResult GetFriend(Guid idA, Guid idB);

        ServiceResult GetRequestedFriend(Guid userId,int index, int count);

        ServiceResult GetUserFriends(Guid userId, int index, int count);

        ServiceResult GetCountRequestedOfUser(Guid userId);
    }
}
