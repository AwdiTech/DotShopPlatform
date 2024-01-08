# DotShopPlatform

## Introduction

DotShopPlatform is a full-stack e-commerce application that demonstrates a complete user journey from registration and login to product browsing, cart management, and checkout. This project showcases my skills in creating a secure, scalable, and user-friendly shopping platform using modern web development technologies.

## Features
- User Authentication: Secure login and registration system.
- Product Browsing: Explore products by brands.
- Cart Management: Add items to cart and manage them.
- Checkout Process: Review cart contents and confirm orders.

## Technologies Used
- **Frontend**: Vue.js with Vite, PrimeVue UI Components
- **Backend**: .NET 6, Entity Framework Core, SQL Server
- **APIs**: RESTful API design


---
## Installation and Setup
To run this project locally:

1. Clone the repository:
```bash
git clone https://github.com/AwdiTech/DotShopPlatform.git
```

2. Navigate to the backend directory and restore dependencies:
```bash
cd dotshopplatform-backend/DotShopPlatform
dotnet restore
```

3. Run the migrations to set up the database, and then start the backend server:
```bash
dotnet ef database update
dotnet run
```

4. Navigate to the frontend directory and install npm packages:
```bash
cd ../dotshopplatform-frontend/dotshopplatform-vite-app
npm install
```

5. Run the frontend development server:
```bash
npm run dev
```

Then simply navigate to localhost:3000 or whichever configured port used for the for Front-End.


---
## Using the Application

Here's how to use the application with accompanying screenshots:

### Login Page

User can log in using their credentials.  
![](https://i.imgur.com/ozGZk4v.png)

### Browsing Products

Users can browse products by selecting different brands.  
![](https://i.imgur.com/TuJnWI0.png)

### Adding to Cart

Items can be added to the cart for purchase.  
![](https://i.imgur.com/sJOlg5t.png)

### Viewing Cart and Checkout

Users can review their cart and proceed to checkout.  
![](https://i.imgur.com/vpZammx.png)

### Order Confirmation

Confirmation message upon successful order submission.  
![](https://i.imgur.com/yC97P0b.png)

### Registration

New users can register an account to start shopping.  
![](https://i.imgur.com/9MHAd8p.png)

---
## Code Overview

The project is structured as follows:

- `dotshopplatform-backend`: .NET backend application
- `dotshopplatform-frontend`: Vue.js frontend application
- `data`: JSON files for initial data seeding

---
## Future Improvements

Future updates will include:

- Transitioning the frontend to a polished React.js version.
- Implementing advanced product filtering and searching.
- Enhancing the user profile with order history and tracking.

