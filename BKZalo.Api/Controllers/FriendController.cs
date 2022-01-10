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
    public class FriendController : ControllerBase
    {
        #region Delare

        protected IBaseService<Friend> _friendService;
        protected IFriendService _friendService1;
        protected IBaseService<Account> _accountService;

        #endregion


        #region Consstructor

        public FriendController(IBaseService<Friend> friendService, IBaseService<Account> accountService, IFriendService friendService1)
        {
            _friendService = friendService;
            _friendService1 = friendService1;
            _accountService = accountService;
        }

        #endregion

        [HttpPost("set_request_friend")]
        public IActionResult SetRequestFriend([FromQuery] Guid user_id)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            if (user_id.CompareTo(acc.AccountId) == 0)
            {
                return StatusCode(400, new ResponseModel(1004, "Cannot set friend request yourself", null));
            }
            var a = _accountService.GetById(user_id).Response.Data;
            if(a == null)
            {
                return StatusCode(400, new ResponseModel(9995, "User is not validated", null));
            }
            ServiceResult serviceResult;
            try
            {
                serviceResult = _friendService.Add(new Friend(acc.AccountId, user_id, false));

            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseModel(1004, "Duplicate friend request!", null));
            }
            if(serviceResult.StatusCode == 201)
            {
                var sr = _friendService1.GetCountRequestedOfUser(acc.AccountId);
                return StatusCode(sr.StatusCode, sr.Response);
            }
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }


        [HttpPost("set_accept_friend")]
        public IActionResult SetAcceptFriend([FromQuery] Guid user_id, [FromQuery] bool is_accept)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            if (user_id.CompareTo(acc.AccountId) == 0)
            {
                return StatusCode(400, new ResponseModel(1004, "Cannot accept friend yourself", null));
            }
            var sr = _friendService1.GetFriend(user_id, acc.AccountId);
            if(sr.StatusCode != 200)
            {
                return StatusCode(sr.StatusCode,sr.Response);
            }
            Friend friend = (Friend)sr.Response.Data;
            if (is_accept)
            {
                friend.IsFriend = true;
                var serviceResult = _friendService.Update(friend, friend.FriendId);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                var serviceResult = _friendService.Delete(friend.FriendId);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
        }


        [HttpGet("get_requested_friend")]
        public IActionResult GetRequestedFriend([FromQuery] int index, [FromQuery] int count)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            var serviceResult = _friendService1.GetRequestedFriend(acc.AccountId,index,count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }


        [HttpGet("get_user_friends")]
        public IActionResult GetUserFriends([FromQuery] Guid user_id, [FromQuery] int index, [FromQuery] int count)
        {
            if(user_id.CompareTo(Guid.Empty)==0)
            {
                var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
                var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
                var serviceResult = _friendService1.GetUserFriends(acc.AccountId,index,count);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                var serviceResult = _friendService1.GetUserFriends(user_id,index,count);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
        }

    }
}
