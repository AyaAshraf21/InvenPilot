# InvenPilot

**Inventory Management REST API built with ASP.NET Core and Clean Architecture**

InvenPilot is an inventory management backend API built with **ASP.NET Core 8**. The project handles products, categories, customers, suppliers, and orders, with authentication, validation, inventory tracking, and order business rules.

The main goal of the project was not just to build CRUD endpoints, but to structure the application in a way that keeps business logic separated, testable, and easy to extend.

---

## Features

### Authentication & Authorization

* User registration
* User login
* JWT-based authentication
* ASP.NET Core Identity
* Secure password handling through Identity
* Role-based authorization
* Admin and Employee roles
* Role-based access control for protected endpoints

### Products

* Create, update, delete, and retrieve products
* Product search
* Filter by:

  * Category
  * Price
  * Quantity
  * Stock status
* Sort by:

  * Name
  * Price
  * Quantity
* Pagination
* Category existence validation
* Duplicate product name validation

### Categories

* Create, update, delete, and retrieve categories
* Category search
* Sorting by name
* Pagination
* Duplicate name validation

### Customers

* Create, update, delete, and retrieve customers
* Search by:

  * Name
  * Email
  * Phone
* Sorting
* Pagination
* Validation and exception handling

### Suppliers

* Create, update, delete, and retrieve suppliers
* Search by:

  * Name
  * Email
  * Phone
* Sorting
* Pagination
* Validation and exception handling

### Orders

* Create orders for both **Sales** and **Purchases**
* Customer validation for sales orders
* Supplier validation for purchase orders
* Product existence validation
* Stock availability validation for sales
* Automatic stock updates when creating orders
* Order status management
* Filtering by:

  * Order status
  * Order type
  * Customer
  * Supplier
* Sorting by date
* Pagination

---

## Order Business Rules

Orders contain the main business logic of the application.

### Creating an order

**Sale Order**

* Requires a valid customer.
* Every product must exist.
* The requested quantity must be available.
* Product stock is decreased when the order is created.

**Purchase Order**

* Requires a valid supplier.
* Every product must exist.
* Product stock is increased when the order is created.

### Updating order status

Orders start with the `Pending` status.

The following transitions are supported:

```text
Pending → Completed
Pending → Cancelled
```

When a pending order is cancelled, the inventory change made during order creation is reversed.

For example:

```text
Sale:
Create order  → Stock decreases
Cancel order  → Stock increases back

Purchase:
Create order  → Stock increases
Cancel order  → Stock decreases back
```

Completed and cancelled orders cannot be updated.

This prevents inventory from being modified incorrectly after an order has reached a final state.

---

## Architecture

The project follows **Clean Architecture** and is divided into four main projects:

```text
InvenPilot
│
├── InvenPilot.API
│
├── InvenPilot.Application
│
├── InvenPilot.Domain
│
├── InvenPilot.Infrastructure
|
└── InvenPilot.Tests

```

### Domain

Contains the core business entities and domain concepts.

Examples:

* Product
* Category
* Customer
* Supplier
* Order
* OrderItem

The Domain layer does not depend on the other application layers.

### Application

Contains the application logic and use cases.

This layer includes:

* Commands
* Queries
* DTOs
* Handlers
* Validators
* Repository interfaces
* Unit of Work interface
* Application exceptions
* Mapping profiles
* MediatR pipeline behaviors

### Infrastructure

Contains implementations for external concerns such as:

* Entity Framework Core
* SQL Server
* Repositories
* Unit of Work
* ASP.NET Core Identity
* Database configuration

### API

Contains the HTTP layer:

* Controllers
* Middleware
* Authentication configuration
* Dependency Injection configuration
* Swagger/OpenAPI configuration


### Testing

The application includes **unit tests using xUnit and Moq**.

The tests focus mainly on application behavior and business rules rather than testing framework internals.

Important scenarios covered include:

* Duplicate product validation
* Missing category validation
* Successful product creation
* Product update validation
* Order creation validation
* Customer/supplier validation
* Stock availability
* Order status transitions
* Stock restoration when cancelling orders
* Invalid order status changes
* Query and response handling

The goal of the tests is to make sure important business rules continue to work correctly when the application changes.

---

## Design Patterns & Practices

The project uses several patterns and practices commonly used in modern .NET applications.

### CQRS

Commands and queries are separated using **MediatR**.

Examples:

```text
CreateProductCommand
UpdateProductCommand
GetAllProductsQuery
GetProductByIdQuery
```

This keeps each use case focused on a single responsibility.

### MediatR

MediatR is used to dispatch commands and queries to their corresponding handlers.

### Repository Pattern

Repositories abstract data access from the Application layer.

Examples:

```text
IProductRepository
ICategoryRepository
ICustomerRepository
ISupplierRepository
IOrderRepository
```

### Unit of Work

The Unit of Work pattern is used to coordinate database changes and commit related operations together.

For example, creating an order can involve:

```text
Create Order
     +
Update Product Stock
     ↓
SaveChangesAsync()
```

### Dependency Injection

Dependencies such as repositories, Unit of Work, AutoMapper, and MediatR are provided through ASP.NET Core's built-in Dependency Injection container.

### AutoMapper

AutoMapper is used to reduce repetitive entity/DTO mapping code.

For example:

```csharp
CreateMap<Product, ProductResponseDTO>();
CreateMap<ProductDTO, Product>();
```

### FluentValidation

Request validation is implemented using FluentValidation and integrated into the MediatR pipeline.

### Global Exception Handling

A global exception middleware handles application exceptions and returns consistent HTTP responses.

Handled scenarios include:

* Validation errors
* Not found resources
* Duplicate resources
* Invalid credentials
* Bad requests

---

## Data Access

The project uses:

* **Entity Framework Core 8**
* **SQL Server**
* Code First approach
* EF Core migrations

Database changes are managed through migrations rather than manually creating database tables.

---

## Validation & Error Handling

The API uses validation at the application boundary and centralized exception handling.

Examples of handled errors:

```text
400 Bad Request
401 Unauthorized
404 Not Found
409 Conflict
```

Examples include:

* Invalid request data
* Missing required customer/supplier
* Non-existing category
* Non-existing product
* Duplicate product/customer/supplier/category
* Insufficient product stock
* Invalid order status transition

---

## Pagination, Filtering & Sorting

List endpoints support common querying capabilities.

Example:

```http
GET /api/products?page=1&perPage=10
```

Filtering and sorting can be combined with pagination depending on the endpoint.

This approach keeps large collections manageable and avoids returning unnecessary data.

---

## Technologies

| Technology              | Usage                   |
| ----------------------- | ----------------------- |
| C#                      | Programming language    |
| ASP.NET Core 8          | Web API                 |
| Entity Framework Core 8 | ORM                     |
| SQL Server              | Database                |
| ASP.NET Core Identity   | User and role management|          |
| JWT                     | Authentication tokens   |
| Role-Based Authorization| Access control          |
| MediatR                 | CQRS / request handling |
| FluentValidation        | Request validation      |
| AutoMapper              | Object mapping          |
| xUnit                   | Unit testing            |
| Moq                     | Mocking dependencies    |
| Swagger / OpenAPI       | API documentation       |
| Git / GitHub            | Version control         |

---

## What I Focused On

While building InvenPilot, I focused on more than implementing endpoints.

The project was built to practice:

* Clean Architecture
* Separation of concerns
* CQRS with MediatR
* Repository and Unit of Work patterns
* Dependency Injection
* Entity Framework Core
* JWT authentication and role-based authorization
* FluentValidation
* Centralized exception handling
* DTO mapping with AutoMapper
* Pagination, filtering, and sorting
* Inventory-related business rules
* Unit testing and mocking

The order workflow was designed specifically to demonstrate how business rules can affect multiple entities in a single operation, particularly order status changes and product stock.

---

## Future Improvements

Some areas that could be added in a future version include:

* Refresh tokens
* Structured application logging
* Caching
* More extensive integration tests
* Containerization with Docker
* Deployment to a cloud environment
