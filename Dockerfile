#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
COPY . .
RUN dotnet restore
	
FROM build AS publish
RUN dotnet publish . -c Release -o out
#RUN ls -l out

FROM base AS final
WORKDIR /app
COPY --from=publish /app/out .
RUN chmod +x /app/RaspberrySensor.dll

ENTRYPOINT ["dotnet", "RaspberrySensor.dll"]
