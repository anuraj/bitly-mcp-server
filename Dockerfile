FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY ./src/ /app/

RUN dotnet restore

RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "BitlyMcpServer.dll"]