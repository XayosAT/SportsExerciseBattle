// See https://aka.ms/new-console-template for more information
using SportsExercise.Models;
using SportsExercise.DAL;
using SportsExercise.BLL;
using SportsExercise.API.Routing;
using Npgsql;
using System.Net;
using SportsExercise.HttpServer;
using SportsExercise;

var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=SEB";

IUserDao userDao = new DatabaseUserDao(connectionString);
IScoreboardDao scoreboardDao = new DatabaseScoreboardDao(connectionString);

IUserManager userManager = new UserManager(userDao);
IScoreboardManager scoreboardManager = new ScoreboardManager(scoreboardDao);

var tournamentManager = TournamentManager.GetInstance(userManager);
var router = new MessageRouter(userManager, scoreboardManager, tournamentManager);
var server = new HttpServer(router, IPAddress.Any,10001);
server.Start();