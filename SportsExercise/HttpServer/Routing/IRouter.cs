using SportsExercise.HttpServer.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.HttpServer.Routing
{
    internal interface IRouter
    {
        IRouteCommand? Resolve(HttpRequest request);
    }
}
