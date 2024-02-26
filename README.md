# BookStore

## Overview

BookStore is a DotNet Web API built on ASP.NET Core using DotNet Core 6. It provides a set of functionalities for managing users, books, and authors in an online bookstore. The application uses Entity Framework 6 for data access and MySQL as the SQL database. Additionally, it incorporates JWT authentication for secure access and email verification for user registration.

## Technologies Used

- ASP.NET Core Web API
- DotNet Core 6
- Entity Framework 6
- MySQL (SQL Database)
- MVC Architecture

## Features

1. **User Management:**
   - Register new users with email verification.
   - Authenticate users using JWT tokens.
   - Manage user profiles.

2. **Book Management:**
   - Add, update, and delete books.
   - Retrieve a list of books.
   - View book details.

3. **Author Management:**
   - Add, update, and delete authors.
   - Retrieve a list of authors.
   - View author details.

## Getting Started

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/yourusername/BookStore.git
   cd BookStore
2. **Database Setup:**

    a. Configure the MySQL connection string in `appsettings.json`.
    b. Run Entity Framework migrations:
        ```bash
        dotnet ef database update
3. **Run the Application:**
    ```bash
    dotnet run
4. **API Endpoints:**
  - User API: /api/User
  - Book API: /api/Book
  - Author API: /api/Author
## Authentication:
To access protected routes, include the JWT token in the Authorization header of your HTTP requests.

## MVC Architecture:
The application follows the Model-View-Controller (MVC) architectural pattern for better organization and separation of concerns. The structure includes:

  - Controllers: Handle incoming HTTP requests, process user input, and interact with the Model.
  - Models: Represent the data and business logic of the application.
  - Views: Render the user interface.
## Contributing:
Feel free to contribute to the development of BookStore by opening issues and pull requests.

## License:
This project is licensed under the MIT License.
---
Thank you for choosing BookStore for your online bookstore needs! If you have any questions or concerns, please don't hesitate to reach out.

Happy coding! 📚🚀

