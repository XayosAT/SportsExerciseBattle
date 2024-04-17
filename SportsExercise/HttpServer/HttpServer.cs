using SportsExercise.HttpServer.Routing;
using SportsExercise.HttpServer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.HttpServer
{
    internal class HttpServer
    {
        private readonly IRouter _router;
        private readonly TcpListener _listener;
        private bool _listening;

        public HttpServer(IRouter router, IPAddress address, int port)
        {
            _router = router;
            _listener = new TcpListener(address, port);
            _listening = false;
        }

        public void Start()
        {
            _listener.Start();
            _listening = true;

            while (_listening)
            {
                Console.WriteLine("Waiting for a connection...");
                var client = _listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        public void Stop() 
        { 
            _listening = false;
            _listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                var clientHandler = new HttpClientHandler(client);
                var request = clientHandler.ReceiveRequest();
                HttpResponse response;

                if (request is null)
                {
                    response = new HttpResponse(StatusCode.BadRequest);
                }
                else
                {
                    
                    try
                    {
                        Console.WriteLine("Resolving request...");
                        var command = _router.Resolve(request);
                        Console.WriteLine("Command resolved");
                        
                        if (command is null)
                        {
                            response = new HttpResponse(StatusCode.BadRequest);
                        }
                        else
                        {
                            response = command.Execute();
                        }
                        
                    }
                    catch (RouteNotAuthenticatedException)
                    {
                        response = new HttpResponse(StatusCode.Unauthorized);
                    }
                    
                }

                clientHandler.SendResponse(response);
            }
            finally
            {
                Console.WriteLine("Closing client...");
                client.Close();
            }
            
        }
    }
}
