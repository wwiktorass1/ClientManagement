using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using ClientManagement.Data;
using ClientManagement.Models;


namespace ClientManagement.Modules
{
    public class ClientModule : NancyModule
    {
        private readonly Database _database;

        public ClientModule() : base("/clients")
        {
            _database = new Database("Data Source=clients.db;Version=3;");

            // Grąžinti visus klientus
            Get["/"] = _ => {
                var clients = _database.GetAllClients().ToList();
                var json = JsonConvert.SerializeObject(clients);

                return Response.AsText(json, "application/json");
            };
            
            // Grąžinti konkretų klientą pagal ID
            Get["/client/{id:int}"] = parameters =>
            {
                int id = parameters.id;
                var client = _database.GetClientById(id);
                var json = JsonConvert.SerializeObject(client);
                if (client == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return Response.AsText(json, "application/json");
            };
            
            // Pridėti naują klientą
            Post["/client"] = _ =>
            {
                var client = this.Bind<Client>();
                _database.AddClient(client);
                return HttpStatusCode.Created;
            };
            
            // Redaguoti esamą klientą
            Put["/client/{id:int}"] = parameters =>
            {
                int id = parameters.id;
                var client = this.Bind<Client>();
                client.Id = id;
                _database.UpdateClient(client);
                return HttpStatusCode.NoContent;
            };
            
            // Ištrinti klientą
            Delete["/client/{id:int}"] = parameters =>
            {
                int id = parameters.id;
                _database.DeleteClient(id);
                return HttpStatusCode.NoContent;
            };

            // Peržiūrėti su klientu atliktų veiksmų isoriją
            Get["/client/{id:int}/history"] = parameters =>
            {
                int id = parameters.id;
                var history = _database.GetActionHistory(id);
                return Response.AsJson(history);
            };
        }
    }
}