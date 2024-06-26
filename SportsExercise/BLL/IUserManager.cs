﻿using SportsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.BLL
{
    public interface IUserManager
    {
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
        User GetUserByAuthToken(string authToken);
        Profile? FetchProfile(string username);
        void UpdateProfile(string username, Profile profile);
        Stats? FetchStats(string username);
        Record[]? FetchRecords(string username);
        void InsertEntry(string username, Entry entry);
        void UpdateElo(string username, int elo);
    }
}
