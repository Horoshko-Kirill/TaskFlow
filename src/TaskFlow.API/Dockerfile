FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY src/TaskFlow.API/TaskFlow.API.csproj src/TaskFlow.API/
COPY src/TaskFlow.Application/TaskFlow.Application.csproj src/TaskFlow.Application/
COPY src/TaskFlow.Domain/TaskFlow.Domain.csproj src/TaskFlow.Domain/
COPY src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj src/TaskFlow.Infrastructure/

RUN dotnet restore src/TaskFlow.API/TaskFlow.API.csproj

COPY . .

RUN dotnet publish src/TaskFlow.API/TaskFlow.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskFlow.API.dll"]
