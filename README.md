# Congestion Tax Calculator

## Introduction

CongestionTaxCalculator is a project developed using C# and .NET 8. It follows the principles of clean architecture and Domain-Driven Design (DDD). The project includes integration tests for its endpoints to ensure the robustness and reliability of the system.

The project is designed to calculate congestion tax fees for vehicles within the Gothenburg area. It adheres to the congestion tax rules in Gothenburg and handles different tax rules for different cities by reading parameters from an outside data store during runtime.

The application is flexible and can be used in other cities with different tax rules. These tax rules are handled as content outside the application as seed data.

## Getting Started

### Prerequisites

- .NET 8
- C# compiler

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/mohia404/CongestionTaxCalculator
2. Set up your connection string in appsettings.Development.json
   ```sh  
    "ConnectionStrings": {
        "DefaultConnection": "<YOUR_CONNECTION_STRING>"
    }
3. Run the project


### Usage

Following endpoint gets cityName, vehicleName and datePassesToll as inputs and returns tax

##### Endpoint:
```
Post    https://localhost:7239/api/cities/calculate-tax
```

##### Input from body:
```
Content-Type: application/json
{
  "cityName": "Gothenburg",
  "vehicleName": "string",
  "datePassesToll": [
    "2013-07-06T14:47:32.638Z",
    "2013-07-06T15:49:32.638Z",
    "2013-08-07T15:49:32.638Z"
  ]
}
```