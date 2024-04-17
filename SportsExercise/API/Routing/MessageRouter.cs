using Newtonsoft.Json;
using SportsExercise.API.Routing.Users;
using SportsExercise.BLL;
using SportsExercise.HttpServer;
using SportsExercise.HttpServer.Request;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpMethod = SportsExercise.HttpServer.Request.HttpMethod;

namespace SportsExercise.API.Routing
{
    internal class MessageRouter : IRouter
    {
        private readonly IUserManager _userManager;
        
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public MessageRouter(IUserManager userManager)
        {
            _userManager = userManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            Console.WriteLine("In Resolve");
            var checkBody = (string? payload) => payload ?? throw new InvalidDataException();

            try
            {
                Console.WriteLine("Resolving request...try");
                return request switch
                {
                    
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    
                    { Method: HttpMethod.Get, ResourcePath: var path } when path.StartsWith("/users/") => new FetchProfileCommand(_userManager, GetIdentity(request), ExtractUsername(path)),
                    { Method: HttpMethod.Put, ResourcePath: var path } when path.StartsWith("/users/") => new UpdateProfileCommand(_userManager, GetIdentity(request), ExtractUsername(path), checkBody(request.Payload)),
                    
                    _ => null
                };
            }
            catch(InvalidDataException)
            {
                return null;
            }            
        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body is not null ? JsonConvert.DeserializeObject<T>(body) : null;
            return data ?? throw new InvalidDataException();
        }

        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
        }
        
        private string ExtractUsername(string path)
        {
            string username = path.Split("/").Last();
            Console.WriteLine("Extracted username: " + username);
            return username;
        }
    }
}
