using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IRepositories
{
    public interface IFriendRepository
    {
        Friend GetFriend(Guid idA, Guid idB);

        List<Friend> GetUserFriends(Guid userId,int index, int count);

        List<Friend> GetRequestedFriend(Guid userId,int index, int count);

        int GetCountRequestedOfUser( Guid userId);

        int GetCountUserFriends(Guid userId);
    }
}
