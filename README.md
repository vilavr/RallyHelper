# Rally Helper


## Student Information

- **Name**: Vira Lavrova
- **Email**: vilavr@taltech.ee
- **Student Code**: 223682IVSB

## Introduction

Rally Helper for car service park web app. Developed as practice exam for C# course in TalTech

## Commands for db

~~~bash
cd RallyHelper
dotnet ef migrations add InitialCreate --project DAL --startup-project ConsoleApp
dotnet ef database update --project DAL --startup-project ConsoleApp
~~~

## Commands for web pages scaffolding

~~~bash
cd RallyHelper/WebApp
dotnet aspnet-codegenerator razorpage -m Item -dc AppDbContext -udl -outDir Pages/Items --referenceScriptLibraries
~~~
