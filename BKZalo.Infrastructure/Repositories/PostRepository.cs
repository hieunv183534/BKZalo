using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BKZalo.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        public Guid Add(Post post)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var props = post.GetType().GetProperties();
                DynamicParameters parameters = new DynamicParameters();
                Guid id = Guid.Empty;
                foreach (var prop in props)
                {
                    var propName = prop.Name;
                    if(propName == "PostId")
                    {
                        var propValue = Guid.NewGuid();
                        id = propValue;
                        parameters.Add($"@{propName}", propValue);
                    }
                    else
                    {
                        var propValue = prop.GetValue(post);
                        parameters.Add($"@{propName}", propValue);
                    }
                    
                }
                var procName = $"Proc_InsertPost";
                var rowAffect = dbConnection.Execute(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public int CheckNewItem(DateTime lastTimeStamp)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@LastTimeStamp", lastTimeStamp);
                var procName = $"Proc_CheckNewPost";
                var posts = dbConnection.Query<Post>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                var newItems = posts.AsList<Post>().Count;
                return newItems;
            }
        }

        public int GetCommentCount(Guid postId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@PostId", postId.ToString());
                var sql = $"SELECT COUNT(*) AS Cnt FROM comment c WHERE c.PostId = @PostId";
                int commentCount = (int)dbConnection.QueryFirstOrDefault(sql, param: parameters).Cnt;
                return commentCount;
            }
        }

        public List<Post> GetListPost(int index, int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Indexx", index);
                parameters.Add("@Count", count);
                var procName = $"Proc_GetPagingPost";

                var posts = dbConnection.Query<Post>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return (List<Post>)posts;
            }
        }
    }
}
