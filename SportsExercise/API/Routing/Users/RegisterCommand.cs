using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.API.Routing.Users
{
    internal class RegisterCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly Credentials _credentials;

        public RegisterCommand(IUserManager userManager, Credentials credentials)
        {
            _userManager = userManager;
            _credentials = credentials;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                Console.WriteLine("RegisterCommand +++++++++++");
                _userManager.RegisterUser(_credentials);
                Console.WriteLine("RegisterCommand +++++++++++");
                response = new HttpResponse(StatusCode.Created, "User successfully created");
                
            }
            catch (DuplicateUserException)
            {
                Console.WriteLine("DuplicateUserException");
                response = new HttpResponse(StatusCode.Conflict, "User with same username already registered");
            }

            return response;
        }
    }
}
