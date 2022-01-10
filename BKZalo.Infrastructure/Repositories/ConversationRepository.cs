using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BKZalo.Infrastructure.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        public Message GetLastMessage(Guid conversationId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ConversationId", conversationId);
                var procName = $"Proc_GetLastMessage";

                var message = dbConnection.QueryFirstOrDefault<Message>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return message;
            }
        }

        public List<Conversation> GetListConversation(Guid userId, int index, int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Indexx", index);
                parameters.Add("@Count", count);
                parameters.Add("@UserId", userId);
                var procName = $"Proc_GetPagingConversation";

                var conversations = dbConnection.Query<Conversation>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return (List<Conversation>)conversations;
            }
        }

        public List<Message> GetMessages(Guid conversationId, int index, int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ConversationId", conversationId);
                parameters.Add("@Indexx", index);
                parameters.Add("@Count", count);
                var procName = $"Proc_GetMessages";

                var messages = dbConnection.Query<Message>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return (List<Message>)messages;
            }
        }
    }
}
