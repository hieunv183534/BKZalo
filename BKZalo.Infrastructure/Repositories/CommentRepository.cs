using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BKZalo.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        public Guid Add(Comment comment)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var props = comment.GetType().GetProperties();
                DynamicParameters parameters = new DynamicParameters();
                Guid id = Guid.Empty;
                foreach (var prop in props)
                {
                    var propName = prop.Name;
                    if (propName == "CommentId")
                    {
                        var propValue = Guid.NewGuid();
                        id = propValue;
                        parameters.Add($"@{propName}", propValue);
                    }
                    else
                    {
                        var propValue = prop.GetValue(comment);
                        parameters.Add($"@{propName}", propValue);
                    }

                }
                var procName = $"Proc_InsertComment";
                var rowAffect = dbConnection.Execute(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public List<Comment> GetComment(Guid postId, int index, int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@PostId", postId);
                parameters.Add($"@Indexx", index);
                parameters.Add($"@Count", count);
                var procName = $"Proc_GetComment";
                var comments = dbConnection.Query<Comment>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return (List<Comment>)comments;
            }
        }
    }
}
