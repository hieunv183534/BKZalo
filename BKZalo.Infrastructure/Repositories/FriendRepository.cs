using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BKZalo.Infrastructure.Repositories
{
    public class FriendRepository : IFriendRepository
    {

        public int GetCountRequestedOfUser(Guid userId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@IdA", userId);
                var procName = $"Proc_GetCountRequested";
                int count = (int)dbConnection.QueryFirstOrDefault(procName, param: parameters, commandType: CommandType.StoredProcedure).Cnt;
                return count;
            }
        }

        public int GetCountUserFriends(Guid userId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@UserId", userId);
                var procName = $"Proc_GetCountUserFriends";
                int count = (int)dbConnection.QueryFirstOrDefault(procName, param: parameters, commandType: CommandType.StoredProcedure).Cnt;
                return count;
            }
        }

        public Friend GetFriend(Guid idA, Guid idB)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@IdA", idA);
                parameters.Add($"@IdB", idB);
                var procName = $"Proc_GetFriend";
                var friend = dbConnection.QueryFirstOrDefault<Friend>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return friend;
            }
        }

        public List<Friend> GetRequestedFriend(Guid userId,int index,int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@IdB", userId);
                parameters.Add($"@Indexx", index);
                parameters.Add($"@Count", count);
                var procName = $"Proc_GetRequestedFriend";
                var friends = dbConnection.Query<Friend>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return (List<Friend>)friends;
            }
        }

        public List<Friend> GetUserFriends(Guid userId,int index,int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@UserId", userId);
                parameters.Add($"@Indexx", index);
                parameters.Add($"@Count", count);
                var procName = $"Proc_GetUserFriends";
                var friends = dbConnection.Query<Friend>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return (List<Friend>)friends;
            }
        }
    }
}
