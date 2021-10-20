FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build

WORKDIR /app

COPY /BetDotNext ./

RUN dotnet restore

COPY /BetDotNext ./
WORKDIR /app

RUN dotnet publish -c Release -o out
RUN dotnet test --no-build --no-restore -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "BetDotNext.dll"]
