FROM microsoft/dotnet:2.1-sdk-alpine AS builder
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish --configuration Release --output /output

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS runner
WORKDIR /app
COPY --from=builder /output ./
ENTRYPOINT [ "dotnet", "Template.Api.dll" ]
