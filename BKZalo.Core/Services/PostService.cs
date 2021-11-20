using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using BKZalo.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        #region Declare

        IPostRepository _postRepository;

        #endregion

        #region Constructor

        public PostService(IPostRepository postRepository , IBaseRepository<Post> baseRepository) : base(baseRepository)
        {
            _postRepository = postRepository;
        }

        #endregion

        public override ServiceResult Add(Post post)
        {
            try
            {
                // xử lí nghiệp vụ thêm
                var validateRs = Validate(post, "add");

                if (validateRs.Code != -1)
                {
                    _serviceResult.Response = validateRs;
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                // thêm dữ liệu vào db
                var id = _postRepository.Add(post);
                if (!(id.CompareTo(Guid.Empty) == 0))
                {
                    _serviceResult.Response = new ResponseModel(1000, "OK", new { id= id, url = $"/get_post/{id}"});
                    _serviceResult.StatusCode = 201;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(1005, "Unknown Error");
                    _serviceResult.StatusCode = 500;
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
    }
}
