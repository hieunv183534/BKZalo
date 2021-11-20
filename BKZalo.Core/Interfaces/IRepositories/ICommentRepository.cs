using BKZalo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Interfaces.IRepositories
{
    public interface ICommentRepository
    {
        Guid Add(Comment comment);
    }
}
