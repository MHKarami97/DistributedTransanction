#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Oms/Oms.csproj", "Oms/"]
COPY ["Saga.V2/Saga.V2.csproj", "Saga.V2/"]
RUN dotnet restore "Oms/Oms.csproj"
COPY . .
WORKDIR "/src/Oms"
RUN dotnet build "Oms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Oms.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oms.dll"]