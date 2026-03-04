# OTA Microservices Architecture (.NET Core)

This repository contains a sample **OTA (Online Travel Agency) microservices architecture** built using **.NET 8**, following industry-standard backend practices.

## Services Implemented
- **HotelService** – Hotel master data & search
- **BookingService** – Hotel booking flow
- **SearchService** – Aggregated search with Redis caching
- **API Gateway** – Reverse proxy using YARP
- **Redis** – Caching layer
- **Resilience** – Polly (Retry, Timeout, Circuit Breaker)
- **Logging** – Serilog
- **Rate Limiting** – Gateway-level protection

## Architecture Highlights
- Independent databases per service
- API Gateway pattern
- Inter-service communication via HttpClient
- Caching at Search layer
- Fault tolerance using Polly
- Ready for GDS integration (Amadeus)

## Tech Stack
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Redis
- Docker (optional)
- YARP API Gateway
- Serilog + Polly

## How to Run
1. Start Redis (Docker / local)
2. Run HotelService, BookingService, SearchService
3. Run API Gateway
4. Access APIs via Gateway routes

## Purpose
This project was built to understand **real-world OTA architecture** and modern **microservices patterns**.

