FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/EmailSender.Web.Host/EmailSender.Web.Host.csproj", "src/EmailSender.Web.Host/"]
COPY ["src/EmailSender.Web.Core/EmailSender.Web.Core.csproj", "src/EmailSender.Web.Core/"]
COPY ["src/EmailSender.Application/EmailSender.Application.csproj", "src/EmailSender.Application/"]
COPY ["src/EmailSender.Core/EmailSender.Core.csproj", "src/EmailSender.Core/"]
COPY ["src/EmailSender.EntityFrameworkCore/EmailSender.EntityFrameworkCore.csproj", "src/EmailSender.EntityFrameworkCore/"]
WORKDIR "/src/src/EmailSender.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/EmailSender.Web.Host", "src/EmailSender.Web.Host"]
COPY ["src/EmailSender.Web.Core", "src/EmailSender.Web.Core"]
COPY ["src/EmailSender.Application", "src/EmailSender.Application"]
COPY ["src/EmailSender.Core", "src/EmailSender.Core"]
COPY ["src/EmailSender.EntityFrameworkCore", "src/EmailSender.EntityFrameworkCore"]
WORKDIR "/src/src/EmailSender.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "EmailSender.Web.Host.dll"]
