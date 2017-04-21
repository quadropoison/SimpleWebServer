using System;

namespace SimpleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer webServer = new WebServer(Response.SendResponse, "http://localhost:8080/index/");

            webServer.Run();

            Console.WriteLine("A simple webserver. Press any key to quit.");
            Console.ReadKey();

            webServer.Stop();
        }
    }
}
