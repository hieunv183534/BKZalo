using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BKZalo.Api.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ConversationController : ControllerBase
    {

        protected IConversationService _conversationService;
        protected IBaseService<Account> _accountService;

        public ConversationController(IConversationService conversationService, IBaseService<Account> accountService)
        {
            _conversationService = conversationService;
            _accountService = accountService;
        }

        [HttpPost("get_list_conversation")]
        public IActionResult GetListConversation([FromQuery] int index, [FromQuery] int count)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            var serviceResult = _conversationService.GetListConversation(acc.AccountId, index, count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [HttpPost("get_conversation")]
        public IActionResult GetConversation([FromQuery] Guid conversationId, [FromQuery] int index, [FromQuery] int count)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            var serviceResult = _conversationService.GetConversation(acc.AccountId, conversationId, index, count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [HttpDelete("delete_conversation/{conversationId}")]
        public IActionResult GetConversation([FromRoute] Guid conversationId)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;

            Conversation conversation = (Conversation)_conversationService.GetById(conversationId).Response.Data;
            if (conversation.AllMemberId.Contains(acc.AccountId.ToString()))
            {
                var serviceResult = _conversationService.Delete(conversationId);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(400, new ResponseModel(1009, "Not Access!"));
            }
        }
    }
}
