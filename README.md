# Project Design: Vehicle Management System API
## Overview
This project demonstrates the design and implementation of a Vehicle Management System API using ASP.NET Core 6.0 and Azure services. The API enables users to manage their vehicles and related maintenance records. The project leverages Azure services for infrastructure and additional functionalities.

## Objectives
Implement a RESTful API using ASP.NET Core 6.0.
Deploy the API to Azure App Service.
Utilize Azure SQL Database for data storage.
Integrate Azure Application Insights for monitoring and diagnostics.
Architecture
The project is implemented as a microservices architecture, consisting of the following components:

## Vehicle API: An ASP.NET Core RESTful API for managing vehicles.
Maintenance API: An ASP.NET Core RESTful API for managing vehicle maintenance records.
Both APIs are deployed as separate Azure App Services and communicate via HTTP.

## Data Storage
The system uses Azure SQL Database for data storage. The following tables are created:

Vehicles: Stores vehicle information (e.g., make, model, year, VIN).
VehicleMaintenanceRecords: Stores maintenance records for vehicles (e.g., service type, date, cost).
## API Endpoints
### Vehicle API
GET /vehicles: Retrieve a list of all vehicles. <br>
POST /vehicles: Create a new vehicle. <br>
GET /vehicles/{id}: Retrieve a specific vehicle by ID. <br>
PUT /vehicles/{id}: Update a specific vehicle by ID. <br>
DELETE /vehicles/{id}: Delete a specific vehicle by ID. <br>
### Maintenance API
GET /vehicles/{vehicleId}/maintenance: Retrieve a list of all maintenance records for a specific vehicle. <br>
POST /vehicles/{vehicleId}/maintenance: Create a new maintenance record for a specific vehicle. <br>
GET /vehicles/{vehicleId}/maintenance/{id}: Retrieve a specific maintenance record by ID. <br>
PUT /vehicles/{vehicleId}/maintenance/{id}: Update a specific maintenance record by ID. <br>
DELETE /vehicles/{vehicleId}/maintenance/{id}: Delete a specific maintenance record by ID. <br> 

## Authentication and Authorization
The API uses Azure Active Directory (AAD) for authentication and authorization. The API is registered as an application in AAD, and JSON Web Tokens (JWT) are used to secure the endpoints.
<br>
## Monitoring and Diagnostics
Azure Application Insights is integrated into the project to provide monitoring and diagnostics capabilities. This includes logging, performance monitoring, and application tracing.
<br>
## Deployment
The API is deployed to Azure App Service using GitHub Actions for continuous integration and continuous deployment (CI/CD).
