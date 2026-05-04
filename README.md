````markdown name=README.md
# ProductoApi

ProductoApi is a RESTful API designed to manage product information, providing endpoints to create, read, update, and delete product records.

## Features

- Create products
- Retrieve a list or details of products
- Update product information
- Delete products

## Getting Started

### Prerequisites

- [.NET 10](https://dotnet.microsoft.com/download)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/gvalenzuela-dev/ProductoApi.git
   cd ProductoApi
   ```

2. Install dependencies:
   ```bash
   # .NET
   dotnet restore
   ```

### Running the Application

```bash

# For .NET
dotnet run
```

Visit `http://localhost:PORT` in your browser or API client.

## API Endpoints

| Method | Endpoint           | Description           |
|--------|--------------------|----------------------|
| GET    | `/products`        | List all products    |
| GET    | `/products/{id}`   | Get product by ID    |
| POST   | `/products`        | Create new product   |
| PUT    | `/products/{id}`   | Update product by ID |
| DELETE | `/products/{id}`   | Delete product by ID |

_Replace with your actual endpoints_


## License

Distributed under the MIT License. See `LICENSE` for more information.
````
