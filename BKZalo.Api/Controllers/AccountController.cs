using BKZalo.Api.Authentication;
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
    public class AccountController : ControllerBase
    {

        #region Delare

        protected IBaseService<TokenAccount> _tokenAccountService;
        protected IBaseService<Account> _accountService;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        #endregion

        #region Consstructor

        public AccountController( IBaseService<TokenAccount> tokenAccountService, IBaseService<Account> accountService, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _tokenAccountService = tokenAccountService;
            _accountService = accountService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        #endregion

        /// <summary>
        /// Lấy toàn bộ ds
        /// </summary>
        /// <returns></returns>
        /// Author HieuNv
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Account account)
        {
            var token = _jwtAuthenticationManager.Authenticate(account.PhoneNumber, account.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new ResponseModel(1000,"OK",new { token= token }));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] Account account)
        {
            var serviceResult = _accountService.Add(account);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult LogOut([FromHeader] string Authorization)
        {
            var serviceResult = _tokenAccountService.DeleteByProp("Token", Authorization);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("getAccountByPhoneNumber/{sdt}")]
        public IActionResult GetAccountByPhoneNumber([FromRoute] string sdt)
        {
            var serviceResult = _accountService.GetByProp("PhoneNumber", sdt);
            var acc = (Account)serviceResult.Response.Data;
            acc.Password = "xxx";
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost("change_password")]
        public IActionResult ChangePassword([FromQuery] string password, [FromQuery] string newPassword)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            if(acc.Password == password)
            {
                acc.Password = newPassword;
                var serviceResult = _accountService.Update(acc, acc.AccountId);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(400, new ResponseModel(1009, "Password incorrect!"));
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("edit_account")]
        public IActionResult EditAccount([FromBody] Account account)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data;
            var serviceResult = _accountService.Update(account, acc.AccountId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
