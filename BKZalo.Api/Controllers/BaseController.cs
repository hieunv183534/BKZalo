using BKZalo.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKZalo.Api.Controllers
{
    [ApiController]
    public class BaseController<TEntity> : ControllerBase
    {

        #region Delare

        protected IBaseService<TEntity> _baseService;

        #endregion

        #region Consstructor

        public BaseController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }

        #endregion

    }
}
