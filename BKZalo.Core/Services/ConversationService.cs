using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using BKZalo.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Services
{
    public class ConversationService : BaseService<Conversation>, IConversationService
    {
        #region Declare

        IConversationRepository _conversationRepository;
        IBaseRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        public ConversationService(IConversationRepository conversationRepository, IBaseRepository<Conversation> baseRepository, IBaseRepository<Account> accountRepository) : base(baseRepository)
        {
            _conversationRepository = conversationRepository;
            _accountRepository = accountRepository;
        }

        #endregion

        public ServiceResult GetListConversation(Guid userId, int index, int count)
        {
            try
            {
                // xử lí nghiệp vụ lấy dữ liệu
                // lấy tất cả dữ liệu từ db
                var conversations = _conversationRepository.GetListConversation(userId, index, count);
                if (conversations.Count > 0)
                {
                    conversations = CompleteListConversation(conversations, userId);
                    _serviceResult.Response = new ResponseModel(1000, "OK", new { conversations= conversations, numOfNewMessage= 0 });
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(9994, "No data or end of list data");
                    _serviceResult.StatusCode = 204;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }

        public ServiceResult GetConversation(Guid userId,Guid conversationId, int index, int count)
        {
            try
            {
                // xử lí nghiệp vụ lấy dữ liệu
                // lấy tất cả dữ liệu từ db
                var messages = _conversationRepository.GetMessages(conversationId, index, count);
                var conversation = _baseRepository.GetById(conversationId);
                if (conversation !=  null)
                {
                    if (conversation.AllMemberId.Contains(userId.ToString()))
                    {
                        messages = CompleteListMessage(messages);
                        _serviceResult.Response = new ResponseModel(1000, "OK", new { conversation = conversation, messages = messages });
                        _serviceResult.StatusCode = 200;
                        return _serviceResult;
                    }
                    else
                    {
                        _serviceResult.StatusCode = 400;
                        _serviceResult.Response = new ResponseModel(1009, "Not Access!");
                        return _serviceResult;
                    }
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(9994, "No data or end of list data");
                    _serviceResult.StatusCode = 204;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }


        public List<Conversation> CompleteListConversation(List<Conversation> conversations, Guid userId)
        {
            for(int i=0; i< conversations.Count; i++)
            {
                conversations[i].LastMessage = _conversationRepository.GetLastMessage(conversations[i].ConversationId);

                var partnerId = Guid.Parse(conversations[i].AllMemberId.Replace(userId.ToString(),"").Trim());

                conversations[i].Partner = _accountRepository.GetById(partnerId);
                conversations[i].Partner.PhoneNumber = "xxxxxx";
                conversations[i].Partner.Password = "xxxxxx";
            }

            return conversations;
        }

        public List<Message> CompleteListMessage(List<Message> messages)
        {
            for(int i=0; i< messages.Count; i++)
            {
                messages[i].Sender = _accountRepository.GetById(messages[i].SenderId);
                messages[i].Sender.Password = "xxxxxx";
                messages[i].Sender.PhoneNumber = "xxxxxx";
            }
            return messages;
        }
    }
}
