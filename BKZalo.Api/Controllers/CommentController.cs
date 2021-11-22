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
    public class CommentController : ControllerBase
    {
        #region Delare

        protected IBaseService<Comment> _commentService;
        protected ICommentService _commentService1;
        protected IBaseService<Account> _accountService;

        #endregion


        #region Consstructor

        public CommentController(IBaseService<Comment> commentService, IBaseService<Account> accountService)
        {
            _commentService = commentService;
            _accountService = accountService;
        }

        #endregion

        [HttpPost("set_comment")]
        public IActionResult SetComment([FromBody] Comment comment)
        {
            try
            {
                var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
                var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
                comment.AccountId = acc.AccountId;

                var serviceResult = _commentService1.Add(comment);
                if(serviceResult.StatusCode == 201)
                {
                    comment.CommentId = (Guid)serviceResult.Response.Data;
                    comment.Poster = acc;
                    acc.PhoneNumber = "xxxxxx";
                    acc.Password = "xxxxxx"; 
                    return StatusCode(201, new ResponseModel(1000, "OK", comment));
                }
                else
                {
                    return StatusCode(serviceResult.StatusCode,serviceResult.Response);
                }             
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel(9999, "Exception Error", new { msg = ex.Message }));
            }
        }


        [HttpDelete("del_comment/{id}")]
        public IActionResult DeletePost([FromRoute] Guid id)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            Comment preComment = (Comment)_commentService.GetById(id).Response.Data;
            if (preComment.AccountId.CompareTo(acc.AccountId) == 0)
            {
                var serviceResult = _commentService.Delete(id);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(401, new ResponseModel(1009, "Not Access", null));
            }
        }


        [HttpPut("edit_comment/{id}")]
        public IActionResult EditPost([FromBody] Comment comment, [FromRoute] Guid id)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            Comment preComment = (Comment)_commentService.GetById(id).Response.Data;
            if (preComment.AccountId.CompareTo(acc.AccountId) == 0)
            {
                var serviceResult = _commentService.Update(comment, id);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(401, new ResponseModel(1009, "Not Access", null));
            }
        }


        [HttpGet("get_comment")]
        public IActionResult GetComment([FromQuery] Guid postId,[FromQuery] int index, [FromQuery] int count)
        {
            var serviceResult = _commentService1.GetComment(postId, index, count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
