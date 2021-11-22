using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IRepositories;
using BKZalo.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Services
{
    public class FriendService : BaseService<Friend>, IFriendService
    {
        #region Declare

        IFriendRepository _friendRepository;
        IBaseRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        public FriendService(IFriendRepository friendRepository, IBaseRepository<Friend> baseRepository, IBaseRepository<Account> accountRepository) : base(baseRepository)
        {
            _friendRepository = friendRepository;
            _accountRepository = accountRepository;
        }

        #endregion

        public ServiceResult GetFriend(Guid idA, Guid idB)
        {
            try
            {
                var friend = _friendRepository.GetFriend(idA, idB);
                if(friend != null)
                {
                    _serviceResult.Response = new ResponseModel(1000, "OK", friend);
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

        public ServiceResult GetRequestedFriend(Guid userId, int index, int count)
        {
            try
            {
                var friends = _friendRepository.GetRequestedFriend(userId,index,count);
                if(friends.Count > 0)
                {
                    var accounts = CompleteListFriend(friends,userId);
                    _serviceResult.Response = new ResponseModel(1000, "OK", friends);
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

        public ServiceResult GetUserFriends(Guid userId, int index, int count)
        {
            try
            {
                var friends = _friendRepository.GetRequestedFriend(userId,index,count);
                if (friends.Count > 0)
                {
                    var accounts = CompleteListFriend(friends, userId);
                    _serviceResult.Response = new ResponseModel(1000, "OK", friends);
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

        public List<Account> CompleteListFriend(List<Friend> friends, Guid userId)
        {
            List<Account> accounts = new List<Account>();
            for(int i=0; i<friends.Count; i++)
            {
                if (friends[i].IdA.CompareTo(userId) == 0)
                {
                    var user = _accountRepository.GetById(friends[i].IdB);
                    user.Password = "xxxxxx";
                    user.PhoneNumber = "xxxxxx";
                    accounts.Add(user);
                }
                else
                {
                    var user = _accountRepository.GetById(friends[i].IdA);
                    user.Password = "xxxxxx";
                    user.PhoneNumber = "xxxxxx";
                    accounts.Add(user);
                }
            }
            return accounts;
        }
    }
}
