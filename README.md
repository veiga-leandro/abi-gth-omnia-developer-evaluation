# Developer Store API

A .NET 8 based REST API for managing sales records with comprehensive CRUD operations and business rules implementation.

## Table of Contents
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Business Rules](#business-rules)
- [Database Schema](#database-schema)

## Features

- Complete CRUD operations for sales management
- Automatic discount calculations based on quantity
- Role-based access control
- Event publishing system for sales operations
- Docker containerization support
- PostgreSQL database integration

## Prerequisites

- Docker and Docker Compose
- .NET 8.0 SDK (for local development)
- Visual Studio 2022 or VS Code (optional)

## Getting Started

1. Clone the repository:

```bash
git clone <repository-url>
```

2. Navigate to the project directory and run:

```bash
docker-compose up -d
```

The application will be available at:
- HTTP: http://localhost:8080
- HTTPS: https://localhost:8081

## API Documentation

### Authentication
All endpoints require authentication. The API supports role-based access with the following roles:
- Admin
- Manager
- Customer

#### Initial Admin User
The system is seeded with an initial administrator account:
- **Username**: admin
- **Password**: Admin@123

#### Getting Started with Authentication

1. Obtain JWT Token

POST /api/Auth Content-Type: application/json
```json
{ "username": "admin", "password": "Admin@123" }
```

Response:
```json
{ "success": true, "message": "User authenticated successfully", "data": { "token": "eyJhbG...[JWT token]", "expiresIn": 3600 } }
```

2. Use the Token
Add the JWT token to all subsequent requests in the Authorization header:

`Authorization: Bearer eyJhbG...[JWT token]`

#### Making Authenticated Requests
Example of creating a sale with authentication:

```bash
curl -X POST http://localhost:8080/api/Sales 
-H "Authorization: Bearer eyJhbG...[JWT token]" 
-H "Content-Type: application/json" 
-d '{ "branchId": "guid", "items": [ { "productId": "guid", "quantity": 5, "unitPrice": 10.00 } ] }'
```

### Endpoints

#### 1. Create Sale

POST /api/Sales

- **Authorization**: Customer role required
- **Request Body**:

```json 
{ "branchId": "guid", "items": [ { "productId": "guid", "quantity": 5, "unitPrice": 10.00 } ] }
```

- **Response**: `201 Created`

#### 2. Get Sale by ID
GET /api/Sales/{id}
- **Authorization**: Admin, Manager, or Customer role required
- **Response**: `200 OK`

```json
{ "id": "guid", "saleNumber": "string", "date": "datetime", "customerId": "guid", "branchId": "guid", "totalAmount": 50.00, "items": [ { "id": "guid", "productId": "guid", "quantity": 5, "unitPrice": 10.00, "discount": 5.00, "totalAmount": 45.00 } ], "cancelled": false }
```


#### 3. List Sales
GET /api/Sales
- **Authorization**: Admin, Manager, or Customer role required
- **Query Parameters**:
  - `page`: Page number (default: 1)
  - `pageSize`: Items per page (default: 10)
  - `startDate`: Filter by start date
  - `endDate`: Filter by end date
  - `customerId`: Filter by customer
  - `branchId`: Filter by branch

#### 4. Update Sale
PUT /api/Sales/{id}
- **Authorization**: Admin, Manager, or Customer role required
- **Request Body**: Similar to Create Sale

#### 5. Cancel Sale
DELETE /api/Sales/{id}
- **Authorization**: Admin, Manager, or Customer role required
- **Response**: `204 No Content`

#### 6. Cancel Sale Item
DELETE /api/Sales/{saleId}/items/{itemId}
- **Authorization**: Admin, Manager, or Customer role required
- **Response**: `204 No Content`

## Business Rules

### Quantity-Based Discounts
1. 4+ identical items: 10% discount
2. 10-20 identical items: 20% discount
3. Maximum limit: 20 items per product
4. No discounts for quantities below 4 items

### Event Publishing
The system publishes the following events (logged to application log):
- SaleCreated
- SaleModified
- SaleCancelled
- ItemCancelled

## Database Schema

The application uses PostgreSQL with the following connection details:
- Host: localhost
- Port: 5432
- Database: developer_evaluation
- Username: developer
- Password: ev@luAt10n

## Error Handling

The API uses standard HTTP status codes:
- 200: Success
- 201: Created
- 204: No Content
- 400: Bad Request
- 401: Unauthorized
- 403: Forbidden
- 404: Not Found
- 500: Internal Server Error

All error responses follow a standard format:
```json
{ "success": false, "message": "Error description", "errors": ["Detailed error messages"] }
```


## Template README

See [Template README](template-README.md)