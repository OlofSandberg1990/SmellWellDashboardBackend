# Google Sheets API Integration

This project is an API that extracts data from Google Sheets CSV files and provides various endpoints to retrieve keyword rankings and sales data. It is built using .NET 8 with ASP.NET Core, and uses services to read and process data from CSV files. The API is designed to be scalable and extendable, allowing easy access to analytical data from pre-existing CSV files.

## Features

- **Keyword Ranking**: Retrieve keyword rankings from a CSV file.
- **Sales Data by Month**: Fetch sales data for a specific month from CSV.
- **Detailed Monthly Sales**: Get detailed sales data for a specific month.
- **Filtered Sales Data**: Retrieve sales data between two specified dates.
- **Automatic Swagger Redirection**: Automatically redirect users to the Swagger documentation on root access.

## Technologies Used

- **ASP.NET Core**: For building the API.
- **Dependency Injection**: Services are injected and managed via the ASP.NET Core built-in DI container.
- **Swagger/OpenAPI**: For API documentation and testing.
- **CORS**: Configured to allow API calls from specific origins.
- **CSV File Processing**: Reads and processes CSV files asynchronously.

## Project Structure

### Services
The application is designed with a service-oriented architecture where each service has a specific responsibility. The following services are included:

- **CsvReaderService**: Handles reading CSV files asynchronously.
- **KeywordRankingService**: Extracts keyword ranking data from the CSV files.
- **SalesDataService**: Handles sales data extraction, filtering by date ranges, and monthly summaries.
- **MonthMappingService**: Provides utilities to convert between month names, numbers, and paths to CSV files.

### Endpoints
The API exposes the following endpoints:

- **GET /KWRANKING**: Retrieves keyword ranking data from a specified CSV file.
- **GET /SALESRANKING/{month}**: Fetches sales data for a specific month.
- **GET /DETAILEDSALES/{month}**: Retrieves detailed sales data for a particular month.
- **GET /FILTEREDSALES/{startDate}/{endDate}**: Retrieves sales data between the specified start and end dates.
- **GET /**: Redirects to Swagger documentation.

### Swagger Integration
The API uses Swagger for generating the OpenAPI documentation. When accessing the root URL, users are automatically redirected to the Swagger UI, which provides an interactive way to explore and test the API endpoints.

