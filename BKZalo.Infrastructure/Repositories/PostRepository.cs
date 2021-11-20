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
    }
}
