# RESTful webapi 2, net6

## Preparations
 - Pull submodules `git submodule update --init --recursive`
 - Create empty PostgreSQL database 
 - Override default connection string using environment variable
 ``` export RSP_DBCONNECTION="Host=localhost;Database=rest-sharp-postgres;Username=rest-sharp-postgres;Password=rest-sharp-postgres"```
 - Alternatively, run app/tests like this 
 ```RSP_DBCONNECTION="Host=localhost;Database=rest-sharp-postgres;Username=rest-sharp-postgres;Password=rest-sharp-postgres" dotnet run/test```
 - Alternatively, modify appsettings.jsons

## Projects in solution
 - Server - main application
 - Server.Test - classic fast unit tests, used to be invoked all the time
 - Server.Integration.Test - slow tests which communicate with the server via http, used to be invoked once in a while
 - Server.ClientLib - client lib, wrapper for http/dav requests from client to server
 - Server.DataLib - as for Client and Server transfert data between each other, data modles are stored here, both client and server use reference to this lib
 - Common - miscelnaouts

## Tests
All tests are based on xUnit

### Server.Integration.Test
Will automatically build and run main app, then connect to it via localhost:5000 and perform api requests.

This tests are relatively slow, so not handy during development process, but nice for final check.

Do not run main app and integration tests on same machine simultaneously, that will cause a conflict.

### Server.Test
Classic unit tests, fast (just CRUD in that sample project, though)

## Protocol
```
/ <- used to check if service is alive
```

### CRUD
```
POST book - create new item
GET book - read collection or collection item
PUT book/{id} - update existing item
DELETE book/{id} - remove existing item
```

### Mark book as read
Can be done better (with many-to-one or many-to-many relations to collection of shelfs), but time is up. 
```
PUT book/{id} pass Book object as json like {
                Id: id,
                Title: "...",
                Description: "...",
                Read: true,
            }
```

### Search
```
GET book/?read=true/false - get list of books which had been read
GET book/?title=exact+match - get list of book with title "exact match"
GET book/?title=like:"partial title" - get list of books where title contains string "partial title"
GET book/?read=true&title=like:"partial title" - get list of books which had been read with title containing string "partial title"
```

## Database
### Interaction with data
Entity Framework Core ORM, code-first. Database schema updated on startup.

### Database server
PostgreSQL

### Config
Defaults, which can be overriten with application.json, which can be overriten with costom config set as environment variable,
and on top of that any parameter be overriten with it's own entironment variables.
Because parameters like database password for production should not be part of the code.

Missing or invalid parameter have to cause human readable error with instruction how to fix that.

Also, it's nice to have an ability to run multiple instances at once (see 12factor.net).

