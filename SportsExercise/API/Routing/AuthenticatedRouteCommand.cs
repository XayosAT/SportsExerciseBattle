using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.API.Routing
{
    internal abstract class AuthenticatedRouteCommand : IRouteCommand
    {
        public User Identity { get; init; }

        protected AuthenticatedRouteCommand(User identity)
        {
            Identity = identity;
        }

        public abstract HttpResponse Execute();
    }
}
