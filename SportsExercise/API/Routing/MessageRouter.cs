using Newtonsoft.Json;
using SportsExercise.API.Routing.Users;
using SportsExercise.API.Routing.Tournament;
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
        private readonly IScoreboardManager _scoreboardManager;
        
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;
        
        private readonly TournamentManager _tournamentManager;

        public MessageRouter(IUserManager userManager, IScoreboardManager scoreboardManager, TournamentManager tournamentManager)
        {
            _userManager = userManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
            _scoreboardManager = scoreboardManager;
            _tournamentManager = tournamentManager;
            
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            //Console.WriteLine("In Resolve");
            var checkBody = (string? payload) => payload ?? throw new InvalidDataException();

            try
            {
                //Console.WriteLine("Resolving request...try");
                
                //Console.WriteLine(request.ResourcePath);
               
                return request switch
                {
                    
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    
                    { Method: HttpMethod.Get, ResourcePath: var path } when path.StartsWith("/users/") => new FetchProfileCommand(_userManager, GetIdentity(request), ExtractUsername(path)),
                    { Method: HttpMethod.Put, ResourcePath: var path } when path.StartsWith("/users/") => new UpdateProfileCommand(_userManager, GetIdentity(request), ExtractUsername(path), checkBody(request.Payload)),
                    
                    {Method: HttpMethod.Get, ResourcePath: var path} when path.StartsWith("/stats") => new FetchStatsCommand(_userManager, GetIdentity(request)),
                    {Method: HttpMethod.Get, ResourcePath: var path} when path.StartsWith("/score") => new FetchScoreboardCommand(_scoreboardManager, GetIdentity(request)),

                    {Method: HttpMethod.Get, ResourcePath: var path} when path.StartsWith("/history") => new FetchHistoryCommand(_userManager, GetIdentity(request)),
                    {Method: HttpMethod.Post, ResourcePath: var path} when path.StartsWith("/history") => new TakePartInTournamentCommand(_tournamentManager, _userManager, GetIdentity(request), checkBody(request.Payload)),
                    {Method: HttpMethod.Get, ResourcePath: var path} when path.StartsWith("/tournament") => new GetTournamentInfoCommand(_tournamentManager, GetIdentity(request)),
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
            Console.WriteLine("Getting identity...");
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
        }
        
        private string ExtractUsername(string path)
        {
            Console.WriteLine("Extracting username...");
            string username = path.Split("/").Last();
            Console.WriteLine("Extracted username: " + username);
            return username;
        }
    }
}
