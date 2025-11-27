# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build 
WORKDIR /src
COPY . .

RUN dotnet restore "UsersMicroservice.sln" --disable-parallel
RUN dotnet publish "src/Users.Api/Users.Api.csproj" -c release -o /app --no-restore

# Runtime stage 
FROM mcr.microsoft.com/dotnet/aspnet:9.0 as runtime
WORKDIR /app
COPY --from=build /app ./

# Expose ports 
EXPOSE 5000

ENTRYPOINT ["dotnet", "Users.Api.dll"]
