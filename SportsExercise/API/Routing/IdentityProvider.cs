using SportsExercise.BLL;
using SportsExercise.HttpServer.Request;
using SportsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.API.Routing
{
    internal class IdentityProvider
    {
        private readonly IUserManager _userManager;

        public IdentityProvider(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public User? GetIdentityForRequest(HttpRequest request)
        {
            User? currentUser = null;

            if (request.Header.TryGetValue("Authorization", out var authToken))
            {
                Console.WriteLine("authToken: " + authToken);
                const string prefix = "Basic ";
                if (authToken.StartsWith(prefix))
                {
                    try
                    {
                        Console.WriteLine("authToken.Substring(prefix.Length): " + authToken.Substring(prefix.Length));
                        currentUser = _userManager.GetUserByAuthToken(authToken.Substring(prefix.Length));
                    }
                    catch(UserNotFoundException ex) 
                    { 
                        Console.WriteLine("UserNotFoundException: " + ex);
                    }
                }
            }

            return currentUser;
        }
    }
}
