FROM microsoft/dotnet:2.2-sdk-alpine AS builder
WORKDIR /app

COPY *.sln .
COPY Simple.WebApi/*.csproj ./Simple.WebApi/
RUN dotnet restore

COPY Simple.WebApi/. ./Simple.WebApi/
RUN dotnet publish --configuration Release --output /dist

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine AS runner
WORKDIR /app
COPY --from=builder /dist .
ENTRYPOINT ["dotnet", "Simple.WebApi.dll"]