using System;
using Nancy;
using Nancy.Hosting.Self;

namespace ClientManagement{
    public class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:1234");
            var hostConfigs = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true }
            };

            using (var host = new NancyHost(hostConfigs, uri))
            {

                try
                {
                    host.Start();
                    Console.WriteLine($"Nancy now listening on {uri}. Press Enter to stop.");
                    Console.ReadLine();
                }
                finally
                {
                    host.Stop();
                }
            }
        }
    }
}