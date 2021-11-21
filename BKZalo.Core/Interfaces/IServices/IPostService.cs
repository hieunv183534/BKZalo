using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IServices
{
    public interface IPostService : IBaseService<Post>
    {
        ServiceResult GetListPost(int index, int count, Guid accountId);

        ServiceResult CheckNewItem(Guid lastId);
    }
}
