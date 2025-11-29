FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Umbrella.Api/Umbrella.Api.csproj", "Umbrella.Api/"]
COPY ["Umbrella.Application/Umbrella.Application.csproj", "Umbrella.Application/"]
COPY ["Umbrella.Domain/Umbrella.Domain.csproj", "Umbrella.Domain/"]
COPY ["Umbrella.Infrastructure/Umbrella.Infrastructure.csproj", "Umbrella.Infrastructure/"]
RUN dotnet restore "Umbrella.Api/Umbrella.Api.csproj"
COPY . .
WORKDIR "/src/Umbrella.Api"
RUN dotnet build "Umbrella.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Umbrella.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Umbrella.Api.dll"]

