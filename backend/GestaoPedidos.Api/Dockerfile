FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GestaoPedidos.Api/GestaoPedidos.Api.csproj GestaoPedidos.Api/
RUN dotnet restore GestaoPedidos.Api/GestaoPedidos.Api.csproj

COPY GestaoPedidos.Api/ GestaoPedidos.Api/
RUN dotnet publish GestaoPedidos.Api/GestaoPedidos.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Biblioteca exigida pelo Npgsql para negociação GSS/Kerberos, ausente na imagem base.
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GestaoPedidos.Api.dll"]
