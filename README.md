# RESTful backend for Goodreads alike application (net 6)

Server.Test - classic fast unit tests, used to be invoked all the time
Server.Integration.Test - slow tests which communicate with the server via http, used to be invoked once in a while
Server.ClientLib - client lib, wrapper for http/dav requests from client to server

# Protocol (lets use documentation first approach here)

books
books?startIndex=0size=20 <- we should be able to limit results on collection
books?title=like:boo <- RHS Colon, [title] LIKE %boo%, if we need ":" symbol, we need to use "\:" or like:"title:with:colon"
books?title=book <- title exactly equal
books?authorId=in:[1,2,3] <- select multiple
books/{id}

I will always map complicated queries into view or stored routine or temporary table, so there wont be any nested results
Filter will apply to resulting 'flat' table.

So each RESTful Resource match some View/Procedure/TemporaryTable

books/{id}/authors <- similar collection

## CRUD
POST books - create new item
GET books?{filter} , books/{id} - read collection or collection item
PUT books/{id} - update existing item
DELETE books/{id} - remove existing item


# Database
I will use Entity Framework ORM, code-first. So no DB installation should be required

# Database server
I will use PostgreSQL

# Config
Defaults, which can be overriten with application.json, which can be overriten with costom config set as environment variable,
and on top of that any parameter be overriten with it's own entironment variables.
Because parameters like database password for production should not be part of the code

Missing or invalid parameter have to case human readable error with instruction how to fix that

Also, it's nice to have an ability to run multiple instances at once (see 12factor.net)


# Investigation of Goodreads
Register, Authentication, Set Goal, Set Genres, Whish List, Recomendations, Choose Reader to follow, 
Search book by title or ISBN
Book Shelves
My Profile, friends, Groups, reading challange (for year),
giveaways, top picks, best books, scan books, settings, help

Book entity:
thumbnail pic, title, series, author, rating, short description, full description, reviews, 
dates read

My Book entity:
bookId, my rating, shelfId ( read, currently reading, want to read )
customShelfId any custom shelf

Shelf entity:
id, title


Review entity:
user, reting, text, likes, dislikes, creationDateTime

Like:
user, reviewId, 

omg, thats a lot. 
I'm going to create universal controller for everything and rely on db entities, via reflection

# Bare minimum to implement
### Unauthorized methods
- Register (code sent to email)
- Confirm register 
- Login

### Authorized user methods
- CRUD for collection available for user

### Admin methods
- CRUD for users
- CRUD for default shelves

### Collections
 - all books
 - my books
 - my shelves
 - default shelves
 - reviews
 - my reviews
 - book reviews
 - likes / dislikes
