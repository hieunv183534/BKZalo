using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IRepositories
{
    public interface IPostRepository
    {
        Guid Add(Post post);

        List<Post> GetListPost(int index, int count);

        int CheckNewItem(DateTime lastTimeStamp);

        int GetCommentCount(Guid postId);
    }
}
