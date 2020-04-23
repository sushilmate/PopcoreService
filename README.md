# PopcoreService

This project has swagger enabled so you can access through https://localhost:44301/swagger/index.html

I have also deployed this service on Azure cloud, you can access this API through https://popcoreservice.azurewebsites.net

## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [Visual Studio Code or Visual Studio 2019](https://visualstudio.microsoft.com/vs/) (version 16.3 or later)
* [.NET Core SDK 3](https://dotnet.microsoft.com/download/dotnet-core/3.0)

### Setup
Follow these steps to get your development environment set up:

  1. Clone the repository
  2. At the root directory, restore required packages by running:
      ```
     dotnet restore
     ```
  3. Next, build the solution by running:
     ```
     dotnet build
     ```
  5. Run dotnet command:
     ```
	 dotnet run
	 ```
  6. Launch [https://localhost:44301/swagger/index.html](https://localhost:44301/swagger/index.html) in your browser to view the API
  
  7. also you can access from Azure Web App https://popcoreservice.azurewebsites.net/swagger/index.html

## Technologies
* .NET Core 3
* ASP.NET Core 3

## Versions
The [master] branch is running .NET Core 3.1. 

## License

No license open to everyone.
