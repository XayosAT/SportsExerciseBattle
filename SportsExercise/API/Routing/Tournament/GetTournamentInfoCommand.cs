using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using Newtonsoft.Json;

namespace SportsExercise.API.Routing.Tournament;


internal class GetTournamentInfoCommand : AuthenticatedRouteCommand
{
    private readonly TournamentManager _tournamentManager;
    
    public GetTournamentInfoCommand(TournamentManager tournamentManager, User identity) : base(identity)
    {
        Console.WriteLine("GetTournamentInfoCommand Constructor");
        _tournamentManager = tournamentManager;
        
    }
    
    public override HttpResponse Execute()
    {
        Console.WriteLine("GetTournamentInfoCommand Execute+++++++++++++");
        // get tournament info
        string info = _tournamentManager.GetTournamentInfo();
        
        // create response with code 200
        HttpResponse response = new HttpResponse(StatusCode.Ok, info);
        
        return response;
    }
}