using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using Newtonsoft.Json;


namespace SportsExercise.API.Routing.Users;

internal class FetchProfileCommand : AuthenticatedRouteCommand
{
    private readonly IUserManager _userManager;
    private readonly string _token;

    public FetchProfileCommand(IUserManager userManager, User identity, string token) : base(identity)
    {
        _userManager = userManager;
        _token = token;
    }
    
    public override HttpResponse Execute()
    {
        Profile? data;
        
        if(Identity.Token != _token)
        {
            return new HttpResponse(StatusCode.Unauthorized, "Token does not match");
        }
        
        try 
        {
            data = _userManager.FetchProfile(_token);
        }
        catch (UserNotFoundException)
        {
            data = null;
        }


        return null;
    }
    
}