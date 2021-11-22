using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using BKZalo.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Services
{
    public class CommentService : BaseService<Comment> , ICommentService
    {
        #region Declare

        ICommentRepository _commentRepository;
        IBaseRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        public CommentService(ICommentRepository commentRepository, IBaseRepository<Comment> baseRepository, IBaseRepository<Account> accountRepository) : base(baseRepository)
        {
            _commentRepository = commentRepository;
            _accountRepository = accountRepository;
        }

        #endregion

        public override ServiceResult Add(Comment comment)
        {
            try
            {
                // xử lí nghiệp vụ thêm
                var validateRs = Validate(comment, "add");

                if (validateRs.Code != -1)
                {
                    _serviceResult.Response = validateRs;
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                // thêm dữ liệu vào db
                var id = _commentRepository.Add(comment);
                if (!(id.CompareTo(Guid.Empty) == 0))
                {
                    _serviceResult.Response = new ResponseModel(1000, "OK", id);
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

        public ServiceResult GetComment(Guid postId, int index, int count)
        {
            try
            {
                // xử lí nghiệp vụ lấy dữ liệu
                // lấy tất cả dữ liệu từ db
                var comments = _commentRepository.GetComment(postId, index, count);
                if (comments.Count > 0)
                {
                    comments = CompleteListComment(comments);
                    _serviceResult.Response = new ResponseModel(1000, "OK", comments);
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

        public List<Comment> CompleteListComment(List<Comment> comments)
        {
            for(int i=0; i<comments.Count; i++)
            {
                var accountId = comments[i].AccountId;
                comments[i].Poster = _accountRepository.GetById(accountId);
                comments[i].Poster.Password = "xxxxxx";
                comments[i].Poster.PhoneNumber = "xxxxxx";
            }
            return comments;
        }
    }
}
