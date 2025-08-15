\# LevelLegal Project



\## Overview

This project is a legal evidence management system built with:

\- .NET Blazor

\- MediatR for CQRS

\- Entity Framework Core

\- SQL Server

\- Bootstrap for UI



\## Features

\- Upload Matters and Evidence CSVs

\- Filter evidence by Matter

\- Display evidence with details (Name, Description, Serial Number, Matter)

\- Logging included (Console / ILogger)

\- CQRS pattern implemented with MediatR



\## Azure Deployment Instructions

1\. Create Azure App Service

2\. Select .NET runtime and OS

3\. Set deployment source (GitHub or Azure CLI)

4\. Configure connection strings

5\. Publish app

6\. Verify app



\## How to Run Locally

1\. Clone the repository

2\. Configure `appsettings.Development.json` with your DB connection

3\. Run migrations:

&nbsp;  ```bash

&nbsp;  dotnet ef database update --startup-project LevelLegal.Web





I chose clean architecture for clarity and maintainability.



Domain Layer: Core entities (Matter, Evidence) and repository interfaces. Contains business rules; no EF Core or Blazor references.



Application Layer: DTOs, CQRS commands/queries, MediatR handlers. Maps entities for the UI and orchestrates workflows (e.g., CSV import).



Infrastructure Layer: EF Core repositories, AppDbContext, migrations, and AppDbContextFactory for design-time.



Web Layer: Blazor pages interact with Application layer via MediatR. Handles file uploads, display, filtering, Bootstrap styling, and logging.



Rationale: CQRS separates reads/writes, DTOs protect domain entities, repositories decouple data access, and Blazor + DI + logging ensures maintainable, interactive UI.

