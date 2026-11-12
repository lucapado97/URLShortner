# URL Shortener API

A simple URL shortener built with .NET 10.0 using minimal APIs.

## Dependencies

- **.NET 10.0 SDK** or later
- **Swashbuckle.AspNetCore** v7.2.0 (for Swagger docs)

## Build Instructions

1. **Clone the repository** (if not already done)

2. **Restore dependencies**:
   dotnet restore

3. **Build the project**:
   dotnet build

4. **Run the application**:
   dotnet run --project URLShortner

   The server will start on `http://localhost:5000` (or the port configured in your environment).

## Usage Examples

### Create a shortened URL for GitHub:
```bash
curl -X PUT "http://localhost:5000/url?url=https://github.com"
```

### Access the shortened URL (follows redirect):
```bash
curl -L "http://localhost:5000/url/000001"
```

### Check redirect without following it:
```bash
curl -I "http://localhost:5000/url/000001"
```

## Swagger UI

The API includes Swagger documentation. After starting the server, visit:

```
http://localhost:5000/swagger
```

## Sample Data

The server initializes with 3 sample URLs on startup:
- `000001` → https://www.google.com
- `000002` → https://www.github.com
- `000003` → https://www.stackoverflow.com

## Notes

- The URL store is thread-safe and shared across all requests
- Duplicate URLs return the same short key
- Keys are 6-digit zero-padded numbers (e.g., `000001`, `000002`)
- Data is stored in memory and will be lost when the server restarts