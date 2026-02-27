FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY BacklogGames/*.csproj ./BacklogGames/
COPY BacklogGames.Bussinnes.Layer/*.csproj ./BacklogGames.Bussinnes.Layer/
COPY BacklogGames.DataAccess.Layer/*.csproj ./BacklogGames.DataAccess.Layer/
RUN dotnet restore BacklogGames/BacklogGames.csproj

COPY . ./
RUN dotnet publish BacklogGames/BacklogGames.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["dotnet", "BacklogGames.dll"]