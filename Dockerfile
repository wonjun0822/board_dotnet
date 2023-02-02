FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY *.csproj .
#RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true
RUN dotnet restore -r linux-musl-x64

COPY . .
#RUN dotnet publish -c Release -o /app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true
RUN dotnet publish -c Release -o /app -r linux-musl-x64 --self-contained false --no-restore
#FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine-amd64
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine-amd64
WORKDIR /app
COPY --from=build /app .

#EXPOSE 80
#EXPOSE 443

# ENV ASPNETCORE_URLS=http://+:80
#ENV ASPNETCORE_URLS="https://+;http://+" 
#ENV ASPNETCORE_HTTPS_PORT=443

ENTRYPOINT ["./board_dotnet"]`