using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using BKZalo.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BKZalo.Core.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        #region Declare

        IPostRepository _postRepository;
        IBaseRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        public PostService(IPostRepository postRepository , IBaseRepository<Post> baseRepository, IBaseRepository<Account> accountRepository) : base(baseRepository)
        {
            _postRepository = postRepository;
            _accountRepository = accountRepository;
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

        public ServiceResult GetListPost(int index, int count, Guid accountId)
        {
            try
            {
                // xử lí nghiệp vụ lấy dữ liệu
                // lấy tất cả dữ liệu từ db
                var posts = _postRepository.GetListPost(index, count);
                if (posts.Count > 0)
                {
                    posts = CompleteListPost(posts, accountId);
                    _serviceResult.Response = new ResponseModel(1000, "OK", posts);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(9994, "No data or end of list data");
                    _serviceResult.StatusCode = 204;
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

        public List<Post> CompleteListPost(List<Post> posts,Guid accountId)
        {
            for(int i=0; i<posts.Count; i++)
            {
                if (!string.IsNullOrEmpty(posts[i].AllAccountIdLiked))
                {
                    posts[i].Like = posts[i].AllAccountIdLiked.Trim().Split(" ").Count();
                    posts[i].IsLiked = posts[i].AllAccountIdLiked.Contains(accountId.ToString());
                }

                // lấy số comment cho post
                posts[i].Comment = _postRepository.GetCommentCount(posts[i].PostId);

                posts[i].MediaUrls = posts[i].AllMediaUrl.Split(" ").ToList<String>();

                posts[i].Author = _accountRepository.GetById(posts[i].AccountId);
                posts[i].Author.Password = "xxxxxx";
                posts[i].Author.PhoneNumber = "xxxxxx";

                posts[i].CanEdit = (posts[i].AccountId.CompareTo(accountId) == 0);
            }
            return posts;
        }

        public ServiceResult CheckNewItem(Guid lastId)
        {
            try
            {

                var post = _baseRepository.GetById(lastId);
                if(post != null)
                {
                    var newItems = _postRepository.CheckNewItem(post.CreatedAt);
                    _serviceResult.Response = new ResponseModel(1000, "OK", new { newItems = newItems });
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(1004, "Parameter value is invalid", null);
                    _serviceResult.StatusCode = 400;
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
