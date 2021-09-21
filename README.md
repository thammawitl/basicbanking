# Basic Banking Application

This application demonstrate basic uses of opening a bank account with functions like deposit, withdraw or trasfering cash to another account

## Tech Stack

Backend -> .NetCore 3.1

- EntityFrameworkCore.PostgreSQL
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.AspNetCore.Mvc.NewtonsoftJson

Frontend -> Angular 12

- @angular/material
- sweetalert2

Db -> PostgreSQL

## How to Run

Commands to get things up and running are:
// on root directory

```

// Run Database

docker-compose -f ./basicbanking.api/docker-compose.yml up

// Run API
cd .\basicbanking.api\
dotnet run

or

dotnet publish ./basicbanking.api/basicbanking.api.csproj -c Release -o dist
dotnet ./basicbanking.api/bin/Release/net5.0/basicbanking.api.dll // this will run inmemory db



// Run Web
cd .\basicbanking.web\
npm install
npm start

```

or simply

```

docker-compose up

```

## Feedback on this Challenge

Thank you for providing the excercise,

It's very interesting and fun in the sametime, as I both get to review and learn new things on .Net and Angular

since the last time I used it was over a year ago.
