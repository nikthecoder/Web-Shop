# Web Shop

## Overview

This is an ASP.NET Core project designed to manage an online store. It includes functionalities for managing products, user roles, cart operations, and database seeding.

## Project Structure

The project includes various directories and functionalities:

### Pages

- **Admin**
  - **OrderManager**: Handles order management functionalities such as viewing orders.
  - **ProductManager**: Contains functionalities for creating, deleting, and editing products.
  - **RoleManager**: Manages user roles within the system.
  - **UserRoles**: Manages user roles based on their permissions.

### Controllers

- **CartreadController**: Manages cart operations, including retrieving cart items and generating orders.

### Identity

- **IdentityHostingStartup**: An implementation of `IHostingStartup` that configures Identity services.

### Data

- **ContextSeed**: Contains methods for database seeding, initializing product data, users, and order statuses.

### Models

- **ApplicationUser**: Inherits from `IdentityUser` and includes user-specific properties such as first name, last name, address, and date of birth.
- **Category**: Represents product categories within the store.
- **Item**: Represents products available in the store.
- **Order** and **OrderStatus**: Models related to orders placed by users and their status.

## Functionality

### Database Seeding

- Utilizes CSV files (`Category.csv`, `Item.csv`, `User.csv`, `OrderStatuses.csv`) to seed the initial data into the database for categories, items, users, and order statuses.

### Cart Operations

- `CartreadController` manages cart-related operations such as retrieving cart items and generating orders based on the cart content. It handles converting JSON data, finding items, and creating orders within the database.

### User and Role Management

- Provides functionalities for managing user roles (`RoleManager` and `UserRoles` pages).
- Allows assigning roles to users based on permissions (`ManageModel`).

### Error Handling

- Includes an `ErrorModel` that generates a unique request ID for each error and provides an error page to track issues.

## Usage

To use the application:

1. Clone the repository.
2. Configure the database connection string in `appsettings.json`.
3. Run database migrations (`dotnet ef database update`) to create the database schema.
4. Run the application using `dotnet run`.

## Dependencies

- `Microsoft.AspNetCore.Identity` for user authentication and authorization.
- `Microsoft.EntityFrameworkCore` for working with the database.
- `Newtonsoft.Json` for JSON serialization/deserialization.

## License

This project is licensed under [Apache-2.0](https://www.apache.org/licenses/LICENSE-2.0).
