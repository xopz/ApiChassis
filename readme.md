# ASP.Net Core WebApi Chassis

This project intent to create a basic ASP.Net Core WebApi Chassis to rampup API development task implementing patterns listed on [Heroku's 12Factor](https://12factor.net/), [Richardson's Microservices.io](https://microservices.io/patterns/index.html), [OpenAPI Specification](https://swagger.io/specification/) and also Microsoft's [implementation](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-implementation) and [design](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design) recomendations.
It's created around .Net Core 2.2 Framework and is prepared to run on Docker stack alongside tools like Configuration Management, Logging and Caching.
The picked patterns and recomendations are listed bellow.

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

## Run

## Colaborate

[![Build Status](https://dev.azure.com/xopz/ApiChassis/_apis/build/status/xopz.ApiChassis?branchName=develop)](https://dev.azure.com/xopz/ApiChassis/_build/latest?definitionId=1&branchName=develop)
