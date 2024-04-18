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
        Console.WriteLine("FetchScoreboardCommand Constructor");
        _scoreboardManager = scoreboardManager;
        
    }
    
    public override HttpResponse Execute()
    {
        Console.WriteLine("FetchScoreboardCommand Execute+++++++++++++");
        Stats[] data = _scoreboardManager.GetScoreboard();
        
        // parse data to json (string) and create repsonse with code 200
        string json = JsonConvert.SerializeObject(data);
        Console.WriteLine(json);
        HttpResponse response = new HttpResponse(StatusCode.Ok, json);
        
        return response;
    }
}