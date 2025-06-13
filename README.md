# 🍕 Fictitious Pizza App

A responsive and feature-rich Angular + .NET 9 web application for managing pizza orders. Built with a focus on usability, sorting, filtering, and a clean dashboard UI.

> This app uses the [Pizza Place Sales CSV dataset](https://www.kaggle.com/datasets/mysarahmadbhat/pizza-place-sales), imported into the database using Entity Framework Core.

----

## Features

### 1. Dashboard
- View the list of orders
- Sort by Order Date, Pizza Count, or Total Amount (Ascending/Descending)
- Paginated orders with customizable page size
- View detailed pizza items per order (size, price, and subtotal)
- Currency formatting, timestamps, and itemized breakdowns
- Fully responsive UI built with Bootstrap 5
  
### 2. Sales Analytics
- Yearly sales summary (based on 2015 data)
- Top 5 best-selling pizzas
- KPIs: Total Revenue, Number of Orders, Best Seller Pizza
- Monthly Sales Trends
- Sales breakdown by category
- All visualized using Chart.js

### Tech Stack
| Frontend      | Backend                | Database                 |
| ------------- | -----------------------| ------------------------ |
| Angular 16+   | ASP.NET Core 9         | MSSQL Server             |
| Bootstrap 5   | Entity Framework Core  |                          |


## Setup Instructions

### 1. Clone the Repository

--bash
git clone https://github.com/vonlouu/FictitousPizzaApp.git
cd FictitousPizzaApp

### 2. Run the Backend (.NET API)

Download the CSV files from Kaggle: https://www.kaggle.com/datasets/mysarahmadbhat/pizza-place-sales
Extract the ZIP file and copy the CSV files (orders.csv, order_details.csv, pizzas.csv, pizza_types.csv).
Navigate to the backend output directory: cd api/PizzaApp.Api/bin/Debug/net9.0
Create a folder named Data and paste the CSV files there.
!!!! Do not rename the CSV files – the application depends on their original names during the seeding process. !!!!
then dotnet run
By default, the API runs at: https://localhost:7061/

### 3. Run the Frontend (Angular App)

cd spa/ClientApp
npm install --legacy-peer-deps
ng serve
By default, the frontend runs at: http://localhost:4200/
If you're using a different host or port, make sure to update the CORS settings in the backend.

Environment Variables (Optional)
If needed, update environment.ts - will vary depending on which server host it.
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7061/api'
};



DASHBOARD UI
![image](https://github.com/user-attachments/assets/daa2d5a7-e31d-4f8b-adde-6a6749a29b1c)

Sales Analytics
![image](https://github.com/user-attachments/assets/cc684719-61af-496c-a4ca-bf164815b5df)


