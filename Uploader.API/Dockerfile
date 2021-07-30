FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Uploader.API/Uploader.API.csproj", "Uploader.API/"]
RUN dotnet restore "Uploader.API/Uploader.API.csproj"
COPY . .
WORKDIR "/src/Uploader.API"
RUN dotnet build "Uploader.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Uploader.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Uploader.API.dll"]
