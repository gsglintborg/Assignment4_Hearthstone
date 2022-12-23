#DOCKERFILE

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy, restore, and build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Assignment4_Hearthstone/Assignment4_Hearthstone.csproj", "Assignment4_Hearthstone/"]
RUN dotnet restore "Assignment4_Hearthstone/Assignment4_Hearthstone.csproj"
COPY . .
WORKDIR "/src/Assignment4_Hearthstone"
RUN dotnet build "Assignment4_Hearthstone.csproj" -c Release -o /app/build

# Publish app
FROM build AS publish
RUN dotnet publish "Assignment4_Hearthstone.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assignment4_Hearthstone.dll"]