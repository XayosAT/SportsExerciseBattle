using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using Newtonsoft.Json;

namespace SportsExercise.API.Routing.Users;

internal class UpdateProfileCommand : AuthenticatedRouteCommand
{
    private readonly IUserManager _userManager;
    private readonly string _username;
    private readonly string _profile;

    public UpdateProfileCommand(IUserManager userManager, User identity, string token, string profile) : base(identity)
    {
        _userManager = userManager;
        _username = identity.Username;
        _profile = profile;
    }
    
    public override HttpResponse Execute()
    {
        Profile? profile = JsonConvert.DeserializeObject<Profile>(_profile);

        if(Identity.Username != _username)
        {
            return new HttpResponse(StatusCode.Unauthorized, "Usernames do not match");
        }

        try 
        {
            _userManager.UpdateProfile(_username, profile);
            return new HttpResponse(StatusCode.Ok, "Profile sucessfully updated");
        }
        catch (UserNotFoundException)
        {
            new HttpResponse(StatusCode.NotFound, "User not found");
        }

        return new HttpResponse(StatusCode.NotFound);
    }
    
    
}