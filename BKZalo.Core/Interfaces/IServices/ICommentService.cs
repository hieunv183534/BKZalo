using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IServices
{
    public interface ICommentService : IBaseService<Comment>
    {
        ServiceResult GetComment(Guid postId, int index, int count);
    }
}
