# FribergCarRentals
Author: David Browning
School: YH Akademin
Class: Advanced C# Programming

## Project Overview
A service for renting cars in Sweden. Users log in and reserve cars for date ranges. Admins can add new cars and edit other users's profiles.

## Technologies
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Dependency injection
- Clean code principles
- Clean architecture principles

## Project Structure
Multiple projects within the overall solution:
- FribergCarRentals.Mvc           # User interface
- FribergCarRentals.Core          # Shared models and interfaces that entire solution depends on
- FribergCarRentals.Data          # Between Services and database; reads and saves data
- FribergCarRentals.Services      # Between WebApi and Data; applies business logic
- FribergCarRentals.WebApi        # Between Mvc and Services; routes requests appropriately

## How to Install
1. Clone the repository
2. Update appsettings.json with "ConnectionStrings": "DefaultConnection" to your own local database
3. Apply migrations with update-database
4. Start the application

## How to Use
- Customer: Register as a new customer
- Customer: Reserve a car
- Customer: Edit your customer info, or edit a reservation
- Admin: Add a new car
- Admin: Edit other users' info (make/remove admin for another user, make/remove customer for another user)
- Admin: Delete a customer's reservation
