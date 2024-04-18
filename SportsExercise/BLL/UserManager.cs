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
            Console.WriteLine("\n Fetching profile for " + username);
            return _userDao.FetchProfile(username);
        }
        
        public void UpdateProfile(string username, Profile profile)
        {
            if (_userDao.UpdateProfile(username, profile) == false)
            {
                throw new UserNotFoundException("User not found");
            }
        }

        public Stats? FetchStats(string username)
        {
            Console.WriteLine("\n Fetching stats for " + username);
            return _userDao.FetchStats(username);
        }
        
        public Record[]? FetchRecords(string username)
        {
            Console.WriteLine("\n Fetching records for " + username);
            return _userDao.FetchRecords(username);
        }

        public void InsertEntry(string username, Entry entry)
        {
            Console.WriteLine("\n Inserting entry for " + username);
            _userDao.InsertEntry(username, entry);
        }
        public void UpdateElo(string username, int elo)
        {
            Console.WriteLine("\n Updating elo for " + username);
            _userDao.UpdateElo(username, elo);
        }
    }
}
