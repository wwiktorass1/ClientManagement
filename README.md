# ClientManagement

## Endpoints

### GET /client
Get all clients.

### GET /client/{id:int}
Get client by ID.

### POST /client/add
Add a new client.

#### Request:
```json
{
  "Name": "Vardas",
  "Age": 12,
  "Comment": "komentaras"
}
```

### PUT /client/{id:int}
Update client by ID.

#### Request:
```json
{
  "Name": "Vardas",
  "Age": 12,
  "Comment": "komentaras"
}
```

### DELETE /client/{id:int}
Delete client by ID.

### GET /client/{id:int}/history
Get actions history of client by ID.
