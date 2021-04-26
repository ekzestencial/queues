#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Beanstalkd.Consumer/Beanstalkd.Consumer.csproj", "Beanstalkd.Consumer/"]
RUN dotnet restore "Beanstalkd.Consumer/Beanstalkd.Consumer.csproj"
COPY . .
WORKDIR "/src/Beanstalkd.Consumer"
RUN dotnet build "Beanstalkd.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Beanstalkd.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beanstalkd.Consumer.dll"]