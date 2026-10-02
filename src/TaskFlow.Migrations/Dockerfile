FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY src/TaskFlow.Migrations/TaskFlow.Migrations.csproj src/TaskFlow.Migrations/
COPY src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj src/TaskFlow.Infrastructure/
COPY src/TaskFlow.Application/TaskFlow.Application.csproj src/TaskFlow.Application/
COPY src/TaskFlow.Domain/TaskFlow.Domain.csproj src/TaskFlow.Domain/

RUN dotnet restore src/TaskFlow.Migrations/TaskFlow.Migrations.csproj

COPY . .

RUN dotnet publish src/TaskFlow.Migrations/TaskFlow.Migrations.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TaskFlow.Migrations.dll"]