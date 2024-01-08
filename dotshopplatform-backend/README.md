# DotShopPlatform Backend

## Overview

The DotShopPlatform backend is a robust and scalable API designed to handle e-commerce operations efficiently. It's built with .NET 8, featuring RESTful endpoints that serve the needs of the frontend application, providing functionalities like user authentication, product browsing, and order processing.

---
## Getting Started
### Prerequisites
- .NET 8 SDK
- SQL Server
- Any preferred IDE (e.g., Visual Studio, VS Code)

### Installation

1. Clone the repository to your local machine.
2. Ensure SQL Server is running.
3. Navigate to the `dotshopplatform-backend/DotShopPlatform` directory.

4. Use the following command to restore the project dependencies:
```
dotnet restore
```

5. To setup the database, run the migrations using:
```
dotnet ef database update
```
- **Note:** Ensure you have installed the EF Core CLI tools if you haven't already. These tools are required to perform database migrations. (`dotnet tool install --global dotnet-ef`)

6. Start the server using:
```
dotnet run
```


## Configuration
The application requires certain configurations before it can run properly. These include:

- `ConnectionStrings`: Add your SQL Server connection string in the `appsettings.json` file.
- `AppSettings`: Set your JWT token secret key here.


## Features
The backend API supports several features including:

- **User Registration and Authentication**: Securely handles user sign-up and sign-in operations.
- **Product Management**: Facilitates product retrieval and categorization by brands.
- **Order Processing**: Manages shopping cart data and processes orders.


---
## API Endpoints

The API provides a series of endpoints, grouped by controller, that are used to interact with the DotShopPlatform application.

### Authentication (LoginController & RegisterController)
- POST `/api/register`
  - Register a new user.
  - Request body should include `email`, `password`, `firstName`, and `lastName`.
- POST `/api/login`
  - Authenticate and receive a JWT token.
  - Request body should include `email` and `password`.

### Brands
- GET `/api/brands`
  - Retrieves all available brands.

### Products
- GET `/api/products/{brandId}`
  - Retrieves all products associated with a given brand ID.

### Orders
- POST `/api/order`
  - Place a new order.
  - Request body should include `email` and an array of `selections` containing product IDs and quantities.

### Customer
- GET `/api/customer/{email}`
  - Retrieve customer information by email.

### Data Loading
- GET `/api/data`
  - Initiates the loading of product and brand data into the database from a predefined JSON source.
  - Typically used to populate the database with initial data set.

### Home
- GET `/api/home`
  - A basic endpoint that might be used for a health check or welcome message when accessing the API root.


---
## Contributing
If you'd like to contribute to the project, please fork the repository and issue pull requests with detailed descriptions of your changes.


## License
This project is licensed under the MIT License - see the LICENSE file for details.


## Acknowledgments
This project makes use of several key technologies and libraries, including:

- **.NET Core**: The framework used to build the backend API.
- **Entity Framework Core**: ORM used for database operations.
- **SQL Server**: Database system used for storing application data.
- **JWT Authentication**: For handling user authentication and securing endpoints.
- **Automapper**: For mapping domain models to DTOs and vice versa.


Special thanks to the entire .NET community for their continuous contributions which make projects like these possible. Additionally, the use of open-source libraries and tools from various authors has greatly facilitated the development process.
