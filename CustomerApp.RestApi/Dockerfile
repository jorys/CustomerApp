FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CustomerApp.RestApi/CustomerApp.RestApi.csproj", "CustomerApp.RestApi/"]
RUN dotnet restore "CustomerApp.RestApi/CustomerApp.RestApi.csproj"
COPY . .
WORKDIR "/src/CustomerApp.RestApi"
RUN dotnet build "CustomerApp.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerApp.RestApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerApp.RestApi.dll"]