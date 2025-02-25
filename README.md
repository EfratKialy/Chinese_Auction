🌟 Chinese Auction - Web API & React Project

📚 Overview

Chinese Auction is a full-stack web application designed for managing a Chinese auction event. The system provides separate functionalities for administrators and customers, ensuring a smooth and secure auction experience. The backend is developed using C# Web API with EF Core, while the frontend is implemented in React.js.

💼 Features

💼 Admin Panel

Administrators can:

Authenticate securely using a username and password. Admin endpoints are protected with JWT authentication and role-based authorization.

Manage donors:

View the list of donors.

Add, update, and delete donor details.

Each donor contains their personal details and a list of their donated gifts.

Filter donors by name, email, or associated gifts.

Manage gifts:

View, add, update, and delete gifts.

Each gift has a category and a donor.

Search gifts by name, donor name, or number of purchases.

Define raffle ticket price per gift.

Manage purchases:

View raffle ticket purchases per gift.

Sort purchases by most expensive gifts or most purchased gifts.

View details of all customers who made purchases.

Conduct the raffle:

Perform a randomized draw for each gift based on the customer purchases.

Generate a report of winners.

Generate a summary report of total earnings.

👤 Customer Features

Customers can:

Register and log in:

Provide name, phone number, and email (validated both on client & server side).

Browse available gifts:

Sort by price or category.

Purchase raffle tickets:

Add gifts to their cart (multiple tickets per gift allowed).

The order remains a draft until the purchase is confirmed.

Admins cannot see unconfirmed drafts.

Once a purchase is confirmed, it cannot be canceled.

View raffle results:

If the raffle has already been conducted, they can see the winner for each gift.

Once the raffle is completed, purchases are disabled.

📝 Development Stack

Backend - C# Web API

.NET Core Web API

Entity Framework Core (EF Core) - Code First

JWT Authentication & Role-Based Authorization

Middleware for authentication and request handling

Error handling & logging

Dependency Injection (DI) for modular and scalable architecture

Frontend - React.js

React 18 with functional components

React Router for navigation

State management with Context API

Styled with TailwindCSS / Material UI

Axios for API communication

🚀 Getting Started

1. Clone the Repository

git clone https://github.com/EfratKialy/ChineseAuction.git
cd ChineseAuction

2. Set Up Backend (C# Web API)

Navigate to the server directory:

cd server

Install dependencies:

dotnet restore

Apply database migrations:

dotnet ef database update

Run the API:

dotnet run

The API should now be available at http://localhost:5000 (or the configured port).

3. Set Up Frontend (React.js)

Navigate to the client directory:

cd client

Install dependencies:

npm install

Start the React application:

npm start

The frontend should now be running at http://localhost:3000.

📈 Database Structure (EF Core - Code First)

The database consists of the following main entities:

User (Admin or Customer)

Donor

Gift (with category and donor reference)

Purchase (customer purchases for raffle tickets)

Winner (record of the raffle results)



💚 Contributing

Contributions are welcome! If you'd like to contribute, please fork the repository and submit a pull request.

🌟 Credits & Contact

Developed by Efrat Kialy ✨For any questions, feel free to contact: 0526528810

🛠️ License

This project is licensed under the MIT License.

