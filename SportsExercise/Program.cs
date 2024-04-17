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
IUserManager userManager = new UserManager(userDao);


var router = new MessageRouter(userManager);
var server = new HttpServer(router, IPAddress.Any,10001);
server.Start();