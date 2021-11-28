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
    public class ReportController : ControllerBase
    {
        #region Delare

        protected IBaseService<Report> _reportService;
        protected IBaseService<Account> _accountService;
        protected IBaseService<Post> _postService;


        #endregion


        #region Consstructor

        public ReportController(IBaseService<Report> reportService, IBaseService<Account> accountService, IBaseService<Post> postService)
        {
            _reportService = reportService;
            _accountService = accountService;
            _postService = postService;
        }

        #endregion

        [HttpPost("report_post")]
        public IActionResult ReportPost([FromQuery] Guid postId, [FromQuery] int subject, [FromQuery] string details)
        {
            if(subject<0 || subject > 4)
            {
                return StatusCode(400, new ResponseModel(1004, "Parameter value is invalid"));
            }
            var sr = _postService.GetById(postId);
            if(sr.StatusCode != 200)
            {
                return StatusCode(400, new ResponseModel(9992, "Post is not existed"));
            }
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            var serviceResult = _reportService.Add(new Report(postId, acc.AccountId, subject, details));
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
