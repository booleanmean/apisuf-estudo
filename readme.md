# apicet-bkend
API CET - Backend


1. docker-compose.exe build

2. heroku login

3. Create app on Heroku

4. Heroku container:login

5. Heroku container:push web -a apicet-bkend

6. Heroku container:release web -a apicet-bkend

7. Heroku logs --tail -a apicet-bkend






Project Packages:
dotnet add package HtmlAgilityPack --version 1.11.42
dotnet add package RestSharp --version 107.3.0


To document Swagger:
dotnet add package Swashbuckle.AspNetCore 


  push:
     branches: [ master ]
   pull_request:
     branches: [ master ]
  workflow_dispatch: