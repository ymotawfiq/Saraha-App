Saraha app allows you to create account and receive messages from anonymous persons, or send messages without any identifications to any registered user also if you logged in you can delete received messages.

How to run project?
You must install dotnet core 8, sql server and Visual studio code or visual studio 2022
to run project in visual studio code
- first you should add migration to create database in sql server
go to (Saraha.Data)
from terminal write (add-migration InitialCreate) if you use visual studio 2022 or (dotnet ef migrations add InitialCreate) if you use visual studio code
then write (update-database) if you use visual studio 2022 or (dotnet ef database update) if you use visual studio code

- second go to (Saraha.Api) and use {dotnet run} command from terminal if you use visual studio code or run it from visual studio 2022

