using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BKZalo.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        protected IBaseService<Account> _accountService;
        protected IBaseService<Conversation> _conversationService;
        protected IBaseService<Message> _messageService;

        public ChatHub(IBaseService<Account> accountService, IBaseService<Conversation> conversationService, IBaseService<Message> messageService)
        {
            _accountService = accountService;
            _conversationService = conversationService;
            _messageService = messageService;
        }

        public async Task JoinChat(Guid conversationId)
        {
            var phoneNumber = Context.GetHttpContext().User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;

            var serviceResult = _conversationService.GetById(conversationId);
            if(serviceResult.StatusCode != 200)
            {
                await Clients.Caller.SendAsync("onmessage", "botSystem", "Error 500!");
            }
            else
            {
                Conversation conversation = (Conversation)serviceResult.Response.Data;

                if (!conversation.AllMemberId.Contains(acc.AccountId.ToString()))
                {
                    await Clients.Caller.SendAsync("onmessage", "botSystem", $"conserId= {conversation.ConversationId} allMember= {conversation.AllMemberId} myId={acc.AccountId}");
                }
                else
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());

                    await Clients.Groups(conversationId.ToString()).SendAsync("onmessage", "botSystem", $"{acc.UserName} join chat");
                }
            }
        }


        public async Task Send(Message message)
        {
            var phoneNumber = Context.GetHttpContext().User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;

            if (message.ConversationId.Equals(Guid.Empty))
            {
                Conversation conversation;
                ServiceResult serviceResult;
                var sr1 = _conversationService.GetByProp("AllMemberId", $"{acc.AccountId} {message.ReceiverId}");
                if(sr1.StatusCode == 204)
                {
                    conversation = new Conversation();
                    conversation.AllMemberId = $"{acc.AccountId} {message.ReceiverId}";
                    conversation.ConversationId = Guid.NewGuid();
                    serviceResult = _conversationService.Add(conversation);
                }
                else if(sr1.StatusCode == 200)
                {
                    conversation = (Conversation)sr1.Response.Data;
                    serviceResult = new ServiceResult();
                    serviceResult.StatusCode = 201;
                }
                else
                {
                    conversation = new Conversation();
                    serviceResult = new ServiceResult();
                    serviceResult.StatusCode = 500;
                }

                if(serviceResult.StatusCode != 201)
                {
                    await Clients.Caller.SendAsync("onmessage", "botSystem", "Error 500!");
                }
                else
                {
                    message.ConversationId = conversation.ConversationId;
                    message.MessageId = Guid.NewGuid();
                    message.SenderId = acc.AccountId;

                    var sr = _messageService.Add(message);
                    if(sr.StatusCode != 201)
                    {
                        await Clients.Caller.SendAsync("onmessage", "botSystem", "Error 500!");
                    }
                    else
                    {
                        await Clients.Group(conversation.ConversationId.ToString()).SendAsync("onCreatedConversation", message);
                    }
                }
            }
            else
            {
                var serviceResult = _conversationService.GetById(message.ConversationId);
                if (serviceResult.StatusCode != 200)
                {
                    await Clients.Caller.SendAsync("onmessage", "botSystem", "Error 500!");
                }
                else
                {
                    Conversation conversation = (Conversation)serviceResult.Response.Data;
                    if (!conversation.AllMemberId.Contains(acc.AccountId.ToString()))
                    {
                        await Clients.Caller.SendAsync("onmessage", "botSystem", "You can't send to this conversation!");
                    }
                    else
                    {
                        message.SenderId = acc.AccountId;
                        message.MessageId = Guid.NewGuid();

                        var sr = _messageService.Add(message);
                        if (sr.StatusCode != 201)
                        {
                            await Clients.Caller.SendAsync("onmessage", "botSystem", $"500 {sr.Response.Data}");
                        }
                        else
                        {
                            await Clients.Groups(message.ConversationId.ToString()).SendAsync("onmessage", $"{acc.UserName}", message);
                        }
                    }
                }
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task DeleteMessage(Guid messageId)
        {
            var phoneNumber = Context.GetHttpContext().User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;

            var serviceResult = _messageService.GetById(messageId);
            if(serviceResult.StatusCode == 200)
            {
                Message message = (Message)serviceResult.Response.Data;
                if (message.SenderId.Equals(acc.AccountId))
                {
                    var sr = _messageService.Delete(messageId);
                    if(sr.StatusCode == 200)
                    {
                        await Clients.Groups(message.ConversationId.ToString()).SendAsync("onmessage", "botSystem",
                                                   new { msg = "Delete message succes", messageId = message.MessageId });
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("onmessage", "botSystem", "Error 500!");
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("onmessage", "botSystem", "Not Access!");
                }
            }
            else if(serviceResult.StatusCode == 204)
            {
                await Clients.Caller.SendAsync("onmessage", "botSystem", "Message does not exist!");
            }
            else
            {
                await Clients.Caller.SendAsync("onmessage", "botSystem", "Error 500!");
            }
        }
    }
}
