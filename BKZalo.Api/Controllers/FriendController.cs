using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected IBaseService<Account> _accountService;

        #endregion


        #region Consstructor

        public FriendController(IBaseService<Friend> friendService, IBaseService<Account> accountService)
        {
            _friendService = friendService;
            _accountService = accountService;
        }

        #endregion


    }
}
