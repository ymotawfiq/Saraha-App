The Short-URL Project provides a method for shortening URLs by mapping original url with
new short url and also integrates seamlessly with third-party services offering similar
functionality

How to run project?
You must install dotnet core 8, sql server and Visual studio code or visual studio 2022
to run project in visual studio code
- first you should add migration to create database in sql server
go to (UrlShorten.Data)
from terminal write (add-migration InitialCreate) if you use visual studio 2022 or (dotnet ef migrations add InitialCreate) if you use visual studio code
then write (update-database) if you use visual studio 2022 or (dotnet ef database update) if you use visual studio code

- second go to (UrlShorten.Api) and use {dotnet run} command from terminal if you use visual studio code or run it from visual studio 2022

