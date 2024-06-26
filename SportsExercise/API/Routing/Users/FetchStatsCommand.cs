using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using Newtonsoft.Json;

namespace SportsExercise.API.Routing.Users;

internal class FetchStatsCommand : AuthenticatedRouteCommand
{
    private readonly IUserManager _userManager;
    

    public FetchStatsCommand(IUserManager userManager, User identity) : base(identity)
    {
        
        _userManager = userManager;
        
    }

    public override HttpResponse Execute()
    {
        Stats? data;

        try
        {
            Console.WriteLine("Identity: " + Identity.Username);
            data = _userManager.FetchStats(Identity.Username);
        }
        catch (UserNotFoundException)
        {
            data = null;
        }

        HttpResponse response;
        if (data == null)
        {
            response = new HttpResponse(StatusCode.NotFound, "User not found");
        }
        else
        {
            // parse data to json (string) and create repsonse with code 200
            string message = "";
            message += "NAME: " + data.Name + ", ELO: " + data.Elo + ", PUSHUPS: " + data.Pushups +
                       " , AVERAGE PUSHUPS: " + data.AveragePushups + "\n";
            
            response = new HttpResponse(StatusCode.Ok, message);
        }

        return response;
    }


}