using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
    public class PostController : ControllerBase
    {

        #region Delare

        protected IBaseService<Post> _postService;
        protected IPostService _postService1;
        protected IBaseService<Account> _accountService;

        #endregion


        #region Consstructor

        public PostController(IBaseService<Post> postService, IPostService postService1, IBaseService<Account> accountService)
        {
            _postService = postService;
            _postService1 = postService1;
            _accountService = accountService;
        }

        #endregion

        [HttpPost("add_post")]
        public IActionResult AddPost([FromBody] Post post)
        {
            try
            {
                var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
                var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
                post.AccountId = acc.AccountId;

                post.AllMediaUrl = string.Empty;
                foreach (var url in post.MediaUrls)
                {
                    post.AllMediaUrl += $"{url} ";
                }
                post.AllMediaUrl = post.AllMediaUrl.Trim();
                post.CanComment = true;

                var serviceResult = _postService1.Add(post);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel(9999, "Exception Error", new { msg = ex.Message }));
            }
        }


        [HttpGet("get_post/{id}")]
        public IActionResult GetPost([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _postService.GetById(id);
                if (serviceResult.StatusCode == 200)
                {
                    var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
                    var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;

                    Post post = (Post)serviceResult.Response.Data;
                    if (!string.IsNullOrEmpty(post.AllAccountIdLiked))
                    {
                        post.Like = post.AllAccountIdLiked.Split(" ").Count();
                        post.IsLiked = post.AllAccountIdLiked.Contains(acc.AccountId.ToString());
                    }

                    post.MediaUrls = post.AllMediaUrl.Split(" ").ToList<String>();

                    post.Author = (Account)_accountService.GetById(post.AccountId).Response.Data;
                    post.Author.Password = "xxxxxx";
                    post.Author.PhoneNumber = "xxxxxx";

                    post.CanEdit = (acc.AccountId.CompareTo(post.AccountId) == 0);
                    return StatusCode(200, new ResponseModel(1000, "OK", post));
                }
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new ResponseModel(9999, "Exception Error", new { msg = ex.Message }));
            }
        }


        [HttpDelete("delete_post/{id}")]
        public IActionResult DeletePost([FromRoute] Guid id)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            Post prePost = (Post)_postService.GetById(id).Response.Data;
            if(prePost.AccountId.CompareTo(acc.AccountId) == 0)
            {
                var serviceResult = _postService.Delete(id);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(401, new ResponseModel(1009, "Not Access", null));
            }
        }


        [HttpPut("edit_post/{id}")]
        public IActionResult EditPost([FromBody] Post post, [FromRoute] Guid id)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            Post prePost = (Post)_postService.GetById(id).Response.Data;
            if (prePost.AccountId.CompareTo(acc.AccountId) == 0)
            {
                post.AllMediaUrl = string.Empty;
                foreach (var url in post.MediaUrls)
                {
                    post.AllMediaUrl += $"{url} ";
                }
                post.AllMediaUrl = post.AllMediaUrl.Trim();

                var serviceResult = _postService.Update(post, id);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(401, new ResponseModel(1009, "Not Access", null));
            }
        }


        [HttpPost("like/{postId}")]
        public IActionResult Like([FromRoute] Guid postId)
        {
            try
            {
                Post post = (Post)_postService.GetById(postId).Response.Data;
                var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
                var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
                if (string.IsNullOrEmpty(post.AllAccountIdLiked))
                {
                    post.AllAccountIdLiked = acc.AccountId.ToString();
                    var serviceResult = _postService.Update(post,postId);
                    if(serviceResult.StatusCode == 201)
                    {
                        return StatusCode(200, new ResponseModel(1000, "OK", new { like = 1 }));
                    }
                    return StatusCode(serviceResult.StatusCode, serviceResult.Response);
                }
                else
                {
                    if (post.AllAccountIdLiked.Contains(acc.AccountId.ToString()))
                    {
                        post.AllAccountIdLiked += " ";
                        post.AllAccountIdLiked.Replace(acc.AccountId.ToString()+" ", "");
                        post.AllAccountIdLiked = post.AllAccountIdLiked.Trim();
                        int like = post.AllAccountIdLiked.Split(" ").Count();
                        return StatusCode(200, new ResponseModel(1000, "OK", new { like = like }));
                    }
                    else
                    {
                        post.AllAccountIdLiked += $" {acc.AccountId.ToString()}";
                        int like = post.AllAccountIdLiked.Split(" ").Count();
                        return StatusCode(200, new ResponseModel(1000, "OK", new { like = like }));
                    }
                }

            }
            catch(Exception ex)
            {
                return StatusCode(500, new ResponseModel(9999, "Exception Error", new { msg = ex.Message }));
            }
        }



        [HttpGet("get_list_post")]
        public IActionResult GetListPost([FromQuery] int index, [FromQuery] int count)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            var serviceResult = _postService1.GetListPost(index, count, acc.AccountId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [HttpGet("check_new_item")]
        public IActionResult CheckNewItem([FromQuery] Guid lastId)
        {
            var serviceResult = _postService1.CheckNewItem(lastId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
