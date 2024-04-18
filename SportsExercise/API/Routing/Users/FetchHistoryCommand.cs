using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using Newtonsoft.Json;

namespace SportsExercise.API.Routing.Users;

internal class FetchHistoryCommand : AuthenticatedRouteCommand
{
    private readonly IUserManager _userManager;
    
    public FetchHistoryCommand(IUserManager userManager, User identity) : base(identity)
    {
        Console.WriteLine("FetchProfileCommand Constructor");
        _userManager = userManager;
        
    }
    
    public override HttpResponse Execute()
    {
        Record[]? data;

        try
        {
            data = _userManager.FetchRecords(Identity.Username);
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
            foreach (Record r in data)
            {
                message += "COUNT: " + r.Count + ", DURATION: " + r.Duration + ", DATE: " + r.Date + "\n";
            }
            
            response = new HttpResponse(StatusCode.Ok, message);
        }

        return response;
    }
}