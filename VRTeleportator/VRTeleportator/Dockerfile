FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["VRTeleportator/VRTeleportator.csproj", "VRTeleportator/"]
RUN dotnet restore "VRTeleportator/VRTeleportator.csproj"
COPY . .
WORKDIR "/src/VRTeleportator"
RUN dotnet build "VRTeleportator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VRTeleportator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VRTeleportator.dll"]