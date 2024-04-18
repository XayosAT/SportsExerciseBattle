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
    private readonly string _username;

    public FetchProfileCommand(IUserManager userManager, User identity, string username) : base(identity)
    {
        
        _userManager = userManager;
        _username = username;
    }
    
    public override HttpResponse Execute()
    {
        Profile? data;

        //Console.WriteLine("______________");
        //Console.WriteLine(Identity.Username);
        //Console.WriteLine(_username);
        
        if(Identity.Username != _username)
        {
            return new HttpResponse(StatusCode.Unauthorized, "Usernames do not match");
        }
        
        
        try 
        {
            data = _userManager.FetchProfile(_username);
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
            message += "NAME: " + data.Name + ", BIO: " + data.Bio + ", IMAGE: " + data.Image + "\n";
            response = new HttpResponse(StatusCode.Ok, message);
        }
        
        return response;

        
    }
    
}