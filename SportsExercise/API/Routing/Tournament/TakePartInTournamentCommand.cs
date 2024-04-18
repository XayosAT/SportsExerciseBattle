using SportsExercise.BLL;
using SportsExercise.HttpServer.Response;
using SportsExercise.HttpServer.Routing;
using SportsExercise.Models;
using System;
using Newtonsoft.Json;

namespace SportsExercise.API.Routing.Tournament;


internal class TakePartInTournamentCommand : AuthenticatedRouteCommand
{
    private readonly TournamentManager _tournamentManager;
    private readonly string _payload; // JSON payload containing push-up data

    public TakePartInTournamentCommand(TournamentManager tournamentManager, User identity, string payload) : base(identity)
    {
        _tournamentManager = tournamentManager;
        _payload = payload;
    }

    public override HttpResponse Execute()
    {
        Entry? entry = JsonConvert.DeserializeObject<Entry>(_payload);
        //print the entry
        Console.WriteLine(entry);
        if (entry == null)
        {
            return new HttpResponse(StatusCode.BadRequest, "Invalid payload");
        }
        try 
        {
            string entryCount = _tournamentManager.AddEntry(Identity.Username, entry);
            return new HttpResponse(StatusCode.Ok, "Entry added successfully. Entry count: " + entryCount);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new HttpResponse(StatusCode.InternalServerError, "Failed to add entry");
        }
        
        
    }
}
