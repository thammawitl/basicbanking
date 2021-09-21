FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY /basicbanking.api/*.csproj ./
COPY /basicbanking.api.test/*.csproj ./
RUN dotnet restore basicbanking.api.csproj

COPY /basicbanking.api ./
RUN dotnet publish basicbanking.api.csproj  -c Release -o dist


FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/dist .

CMD ASPNETCORE_HTTPS_PORT=5001
CMD ASPNETCORE_URLS=http://*:$PORT dotnet basicbanking.api.dll