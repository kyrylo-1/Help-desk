# Help-desk
Check the hosted version of web app [here](https://helpdesktesttask.azurewebsites.net)
## How to setup locally?
- go to _"help-desk-client"_ folder -> run **"npm i"** 
- go to _"Help-desk.API/Help-desk.API"_ folder -> run **"dotnet run"** 
- go to _"help-desk-client"_ folder -> run **"npm run start"** 

## Design overview
![Schema](https://raw.githubusercontent.com/kyrylo-1/Help-desk/master/components.JPG)

## Explanation of major components
- Database has 2 tables: **User** and **Ticket**. Those tables represented as models in my Asp.net core web api
### Authentication
- In order to register user, **AuthController** makes a POST request with name and password and returns a token 
- Token is signed by secret key. Every time request hits **UserController** endpoint, Entity Framework verifies that this token is valid
- During login, program fetch from db user's passwordHash and passwordSalt, computes them and verifies with provided password
### Security
- It a role base application, so when user login, application gets his role for allowing or restricting certain functionality. Restriction happens in the web api and on the client as well.
- Web api can send BadRequest or Unauthorized
- Examples of restrictions on frontend: HelpDeskUser can't delete ticket and can't see tickets from all users. TeamMember can't add tickets



## Client React app screenshot
![Snapshot](https://raw.githubusercontent.com/kyrylo-1/Help-desk/master/Capture.JPG)

## Improvements to do:
- add tests
- format ticket's date on client 
