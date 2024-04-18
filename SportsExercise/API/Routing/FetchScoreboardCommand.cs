using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using Newtonsoft.Json;

namespace SportsExercise.API.Routing;

internal class FetchScoreboardCommand : AuthenticatedRouteCommand
{
    private readonly IScoreboardManager _scoreboardManager;
    
    public FetchScoreboardCommand(IScoreboardManager scoreboardManager, User identity) : base(identity)
    {
       
        _scoreboardManager = scoreboardManager;
        
    }
    
    public override HttpResponse Execute()
    {
       
        Stats[] data = _scoreboardManager.GetScoreboard();
        
        // parse data to json (string) and create repsonse with code 200
        string message = "";
        foreach (Stats s in data)
        {
            message += "USER: " + s.Name + ", PUSH-UPS: " + s.Pushups + ", ELO: " + s.Elo + "\n";
        }
        
        HttpResponse response = new HttpResponse(StatusCode.Ok, message);
        
        return response;
    }
}