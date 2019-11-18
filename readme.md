# ASP.Net Core WebApi Chassis

This project intent to create a basic ASP.Net Core WebApi Chassis to rampup API development task implementing patterns listed on [Heroku's 12Factor](https://12factor.net/), [Richardson's Microservices.io](https://microservices.io/patterns/index.html), [OpenAPI Specification](https://swagger.io/specification/) and also Microsoft's [implementation](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-implementation) and [design](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design) recomendations.
It's created around .Net Core 2.2 Framework and is prepared to run on Docker stack alongside tools like Configuration Management, Logging and Caching.

## Install and Run

The install and run the latest stable version of this template just run the folowing commands:

```sh
dotnet new install Xopz.Templates.Api
dotnet new apichassis --name MyApi
```

## Picked Patterns

Here's a list of patterns already implemented.

| Pattern | Description |
| ------- | ----------- |
| **API Chassis** | This project itself is itented to address facilities proposed in [Richardson's Microservices Chassis Pattern](https://microservices.io/patterns/microservice-chassis.html). Choosen of Service API Project type intented to address 12 Factor's topics [VI](https://12factor.net/processes), [VII](https://12factor.net/port-binding) and [VIII](https://12factor.net/concurrency). |
| **API Metrics** | This project implements healthcheck out of the box via DotNet's 2.2 `app.UseHealthChecks()` |
| **API Versioning** | This project implements URL versioning for APIs |
| **HATEOAS** | *In progress* This project offers an alternative to request data with links as mentioned on [Web Linking specification](https://tools.ietf.org/html/rfc5988.html) and [recomended](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-implementation#provide-links-to-support-hateoas-style-navigation-and-discovery-of-resources) by Microsoft. |
| **OAS 3.0** | This project follows the third version of the OpenAPI Specification for API documentation. |
| **REST** | This API implements REST guidance on processing request so, it follows [HTTP Specification](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html) when sending a response and also follows Microsoft's [recomendantions](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-implementation#processing-requests) |

## Requirements

This template and implementation is based on .Net Core 2.2 so it`s required to have [.Net Core SDK](https://dotnet.microsoft.com/download) or to run this project on an [.Net Core SDK Docker Image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/)

## Build

### Build the Sample API

The avaliable solutions will be inside `source` folder.
Each solution should be able to be opened on Visual Studio or Code for editing, compiling and debuging.
To build the current project from command line use the follwing command:

```sh
dotnet build ./source/ApiChassi/ApiChassi.sln
```

### Run test set

The projects should be provived with a basic set of unit test and they`re included on template.
They can also be executed stand alone with the following command:

```sh
dotnet test ./source/ApiChassi/ApiChassi.sln
```

### Build the Nuget Package

To build the Nuget package we choose to use Docker images.
Theres a predefined scripts to help to build the required Nuget tool Docker image and to pack the Nuget as an template.
To make and build locally (even on Linux or macOS):

```sh
# EXECUTE THE FOLLOWING COMMANDS AT THE ROOT FOLDER
# CREATES A DOCKER IMAGE LOCALLY TO PACK THE SOURCE
./.environment/scripts/make_nuget.cmd
# CREATES THE NUPKG
./.environment/script/make_package.cmd
```

## Run

### Install Nuget package locally

### Install from Nuget.org

## Colaborate

[![Build Status](https://dev.azure.com/xopz/ApiChassis/_apis/build/status/xopz.ApiChassis?branchName=develop)](https://dev.azure.com/xopz/ApiChassis/_build/latest?definitionId=1&branchName=develop)
