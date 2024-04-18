using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExercise.Models;

namespace SportsExercise.DAL
{
    internal interface IUserDao
    {
        User? GetUserByAuthToken(string authToken);
        User? GetUserByCredentials(string username, string password);
        bool InsertUser(User user);
        Profile? FetchProfile(string username);
        bool UpdateProfile(string username, Profile profile);
        Stats? FetchStats(string username);
        Record[]? FetchRecords(string username);
    }
}