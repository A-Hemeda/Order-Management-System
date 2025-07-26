# Order Management System API

A comprehensive .NET 8 Web API for managing orders, products, customers, and users with JWT authentication and role-based authorization.

## 🚀 Features

- **JWT Authentication & Authorization** - Secure API with role-based access control
- **RESTful API Design** - Clean, consistent API endpoints following REST principles
- **DTO Pattern** - Separation of API contracts from domain models
- **AutoMapper Integration** - Automatic mapping between DTOs and domain models
- **Global Exception Handling** - Consistent error responses across the application
- **Swagger Documentation** - Interactive API documentation with JWT support
- **Validation** - Comprehensive input validation using data annotations
- **Logging** - Structured logging for monitoring and debugging
- **In-Memory Database** - Entity Framework Core with in-memory database for development

## 🏗️ Architecture

```
OrderManagementSystem/
├── Controllers/          # API endpoints
├── DTOs/                # Data Transfer Objects
├── Models/              # Domain entities
├── Repositories/        # Data access layer
├── Services/            # Business logic layer
├── Extensions/          # Service collection extensions
├── Middleware/          # Custom middleware
└── Mapping/             # AutoMapper profiles
```

## 🛠️ Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022, VS Code, or any .NET-compatible IDE

## 📦 Installation & Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd OrderManagementSystem
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the API**
   - API Base URL: `https://localhost:7148` or `http://localhost:5287`
   - Swagger UI: `https://localhost:7148` (root URL)

## 🔐 Authentication

The API uses JWT (JSON Web Tokens) for authentication. To access protected endpoints:

1. **Register a user** (POST `/api/Users/register`)
2. **Login** (POST `/api/Users/login`) to get a JWT token
3. **Include the token** in the Authorization header: `Bearer <your-token>`

### User Roles
- **Admin**: Full access to all endpoints
- **Customer**: Limited access (can create orders, view own data)

## 📚 API Endpoints

### Authentication
- `POST /api/Users/register` - Register a new user
- `POST /api/Users/login` - Login and get JWT token

### Products
- `GET /api/Product` - Get all products
- `GET /api/Product/{id}` - Get product by ID
- `POST /api/Product` - Create new product (Admin only)
- `PUT /api/Product/{id}` - Update product (Admin only)
- `DELETE /api/Product/{id}` - Delete product (Admin only)

### Customers
- `POST /api/Customer` - Create new customer
- `GET /api/Customer/{id}/orders` - Get customer orders

### Orders
- `GET /api/Order` - Get all orders (Admin only)
- `GET /api/Order/{id}` - Get order by ID
- `POST /api/Order` - Create new order (Customer only)
- `PUT /api/Order/{id}/status` - Update order status (Admin only)

### Invoices
- `GET /api/Invoice` - Get all invoices (Admin only)
- `GET /api/Invoice/{id}` - Get invoice by ID (Admin only)

## 🔧 Configuration

### JWT Settings
Update `appsettings.json` or use environment variables:

```json
{
  "Jwt": {
    "Key": "YourSecretKeyHere",
    "Issuer": "OrderSystemAPI",
    "Audience": "OrderSystemUsers",
    "DurationInMinutes": 60
  }
}
```

### Environment Variables
For production, use environment variables:
```bash
export JWT__KEY="YourSecretKeyHere"
export JWT__ISSUER="OrderSystemAPI"
export JWT__AUDIENCE="OrderSystemUsers"
export JWT__DURATIONINMINUTES="60"
```

## 🧪 Testing

### Using Swagger UI
1. Navigate to the root URL in your browser
2. Use the interactive Swagger interface to test endpoints
3. Click "Authorize" to add your JWT token

### Using HTTP Client
```bash
# Register a user
curl -X POST "https://localhost:7148/api/Users/register" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password123","role":"Admin"}'

# Login
curl -X POST "https://localhost:7148/api/Users/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password123"}'

# Use the returned token
curl -X GET "https://localhost:7148/api/Product" \
  -H "Authorization: Bearer <your-token>"
```

## 📝 Code Quality Features

- **DTO Pattern**: Clean separation between API and domain models
- **Validation**: Comprehensive input validation with meaningful error messages
- **Logging**: Structured logging for monitoring and debugging
- **Exception Handling**: Global exception handling with consistent error responses
- **Documentation**: XML comments and Swagger documentation
- **Security**: JWT authentication with role-based authorization

## 🔄 Development Workflow

1. **Add new features**:
   - Create DTOs in the `DTOs/` folder
   - Add validation attributes
   - Update AutoMapper profiles in `Mapping/MappingProfile.cs`
   - Implement business logic in services
   - Create controller endpoints with proper documentation

2. **Testing**:
   - Use Swagger UI for manual testing
   - Add unit tests for services and controllers
   - Add integration tests for API endpoints

## 🚀 Deployment

### Local Development
```bash
dotnet run
```

### Production
```bash
dotnet publish -c Release
dotnet OrderManagementSystem.dll
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For support and questions, please open an issue in the repository. 
