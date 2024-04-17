using SportsExercise.DAL;
using SportsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.BLL
{
    internal class UserManager : IUserManager
    {
        private readonly IUserDao _userDao;

        public UserManager(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public User GetUserByAuthToken(string authToken)
        {
            return _userDao.GetUserByAuthToken(authToken) ?? throw new UserNotFoundException("User not found");
        }

        public User LoginUser(Credentials credentials)
        {
            return _userDao.GetUserByCredentials(credentials.Username, credentials.Password) ?? throw new UserNotFoundException("User not found");
        }

        public void RegisterUser(Credentials credentials)
        {
            Console.WriteLine("In UserManager.RegisterUser -----------------");
            var user = new User(credentials.Username, credentials.Password);
            if (_userDao.InsertUser(user) == false)
            {
                Console.WriteLine("DuplicateUserException in RegisterUser");
                throw new DuplicateUserException("User already exists");
            }
        }

        public Profile? FetchProfile(string username)
        {
            return _userDao.FetchProfile(username);
        }
        
        public void UpdateProfile(string username, Profile profile)
        {
            if (_userDao.UpdateProfile(username, profile) == false)
            {
                throw new UserNotFoundException("User not found");
            }
        }
    }
}
