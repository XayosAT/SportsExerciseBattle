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
    private readonly IUserManager _userManager;
    private readonly string _payload; // JSON payload containing push-up data

    public TakePartInTournamentCommand(TournamentManager tournamentManager, IUserManager userManager, User identity, string payload) : base(identity)
    {
        _tournamentManager = tournamentManager;
        _payload = payload;
        _userManager = userManager;
    }

    public override HttpResponse Execute()
    {
        Entry? entry = JsonConvert.DeserializeObject<Entry>(_payload);
        
        
        if (entry == null)
        {
            return new HttpResponse(StatusCode.BadRequest, "Invalid payload");
        }
        try 
        {
            string response = _tournamentManager.AddEntry(Identity.Username, entry);
            _userManager.InsertEntry(Identity.Username, entry);
            return new HttpResponse(StatusCode.Ok, response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new HttpResponse(StatusCode.InternalServerError, "Failed to add entry");
        }
    }
    
}
