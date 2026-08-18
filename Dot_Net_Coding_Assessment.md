# .NET Coding Assessment — Pharmacy Medicine Tracker

## Overview

Client ABC Pharmacy wants a Single Page Application (SPA) built using a Web API and JavaScript/ASP.NET MVC framework to keep track of medicines. The application should support viewing and adding medicine details, and maintain sale records for medicines.

## Medicine Attributes

| Attribute | Type |
|---|---|
| Full Name of the medicine | text |
| Notes | text |
| Expiry Date | Date |
| Quantity | number |
| Price | number, 2 decimal places |
| Brand | text |

## Functional Requirements

### Display the list of medicines available in the system

- The results, showing the medicine attributes **except Notes**, should be displayed in a grid.
- Color indications should follow these rules:
  - **Red background** — medicines with expiry date less than 30 days away
  - **Yellow background** — medicines with quantity in stock less than 10
- The page should have search capability that can query on the medicine name attribute. *(Good to have)*

## Technical Requirements

- Use .NET Core for the API
- Use a JavaScript framework of your choice for the front end
- Store data as JSON on the server side

## Steps to Launch

1. **Create a new project** using the dotnet command (generates a blank console app):
   ```
   dotnet new console --MyConsoleApp
   ```
2. **Build and run** the project:
   ```
   dotnet run --project MyConsoleApp
   ```
3. **Run the Web API or web application**: click "Open Preview" and enter the application URL.
