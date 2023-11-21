#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Grupp6/hakimslivs/hakimslivs.csproj", "Grupp6/hakimslivs/"]
RUN dotnet restore "Grupp6/hakimslivs/hakimslivs.csproj"
COPY . .
WORKDIR "/src/Grupp6/hakimslivs"
RUN dotnet build "hakimslivs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "hakimslivs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "hakimslivs.dll"]