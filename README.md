# ClientManagement#

GET /client - get all clients
GET /client/{id:int} - get client by id
POST /client/add - add client
Request:
{
    "Name": "Vardas",
    "Age": 12,
    "Comment": "komentaras"
}
 
PUT /client/{id:int} - update client by id
Request:
{
    "Name": "Vardas",
    "Age": 12,
    "Comment": "komentaras"
}
 
DELETE client/{id:int} - delete client by id
GET /client/{id:int}/history - get action history by client id
