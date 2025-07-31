E-Commerce Platform

Description
This E-Commerce Platform is a dynamic and scalable web application built during an internship, showcasing robust full-stack development skills. Developed using ASP.NET Boilerplate for the backend and Angular for the frontend, it provides distinct interfaces for administrators and customers. Key features include product management, user management, a statistical dashboard, secure payments via Stripe, email notifications via Mailtrap, and product reviews and ratings. This project demonstrates industry-standard practices for building feature-rich, responsive, and secure e-commerce solutions.
Key Features

Admin and Customer Interfaces: Separate views for administrators (product/user management, dashboard) and customers (shopping, reviews).
Product Management: Admins can add, update, and delete products.
User Management: Admins can view and manage user accounts.
Dashboard: Displays key statistics (e.g., sales, user activity) for admins.
Secure Payments: Integrated Stripe for safe and reliable payment processing.
Email Notifications: Uses Mailtrap to send emails for user registration, order placement, and delivery updates.
Product Reviews and Ratings: Allows customers to rate and review products.
Order Management: Comprehensive tracking and management of customer orders.
Responsive UI: Built with Angular for a seamless, device-friendly experience.

Prerequisites
Before setting up the project, ensure you have the following installed:

.NET Core SDK (version 3.1 or later)
Node.js (version 14.x or later)
Angular CLI (version 12.x or later)
SQL Server or another compatible database
Git for cloning the repository
A Stripe account with API keys for payment integration
A Mailtrap account for email testing and configuration

Installation
Follow these steps to set up the project locally:

Clone the Repository:
git clone https://github.com/your-username/ecommerce-platform.git
cd ecommerce-platform


Restore Backend Dependencies:Navigate to the ASP.NET Boilerplate backend folder and restore dependencies:
cd src/ECommercePlatform.Web
dotnet restore


Install Frontend Dependencies:Navigate to the Angular frontend folder and install dependencies:
cd angular
npm install


Configure the Database:

Update the connection string in appsettings.json (located in src/ECommercePlatform.Web) to point to your SQL Server instance.
Run database migrations:dotnet ef database update




Configure Stripe and Mailtrap:

Update appsettings.json with your Stripe and Mailtrap credentials:"Stripe": {
  "SecretKey": "your-stripe-secret-key",
  "PublishableKey": "your-stripe-publishable-key"
},
"Mailtrap": {
  "Host": "smtp.mailtrap.io",
  "Port": 2525,
  "Username": "your-mailtrap-username",
  "Password": "your-mailtrap-password"
}




Run the Backend:
cd src/ECommercePlatform.Web
dotnet run


Run the Frontend:In a new terminal, navigate to the Angular folder and start the frontend:
cd angular
ng serve


Access the Application:

Backend API: http://localhost:5000
Angular Frontend: http://localhost:4200
Default admin credentials:
Username: admin
Password: 123qwe





Usage
Admin Panel

Log In: Use the credentials (admin/123qwe) to access the admin panel at http://localhost:4200/admin.

Add a Product:

Navigate to the "Products" section.
Click "Add Product" and fill in details (e.g., name, price, description).
Save to list the product.


View Dashboard:

Access the "Dashboard" section to view stats (e.g., total sales, active users).


Manage Users:

Go to the "Users" section to view or manage customer accounts.


API Example (Add Product):
curl -X POST http://localhost:5000/api/products \
-H "Content-Type: application/json" \
-d '{
  "name": "Sample Product",
  "price": 29.99,
  "description": "A sample product for testing.",
  "stock": 100
}'



Customer Interface

Browse Products: Visit http://localhost:4200 to view available products.

Make a Purchase:

Add products to the cart.
Proceed to checkout and complete payment via Stripe.
Receive an order confirmation email via Mailtrap.


Rate and Review:

After purchase, navigate to the product page and submit a rating (1-5 stars) and review.


API Example (Place Order):
curl -X POST http://localhost:5000/api/orders \
-H "Content-Type: application/json" \
-d '{
  "userId": "customer-id",
  "items": [{"productId": "product-id", "quantity": 2}],
  "paymentMethod": "stripe"
}'


Database Connection Issues: Verify the connection string in appsettings.json and ensure SQL Server is running.
Stripe Payment Errors: Confirm that Stripe API keys are correct and test mode is enabled for development.
Mailtrap Email Issues: Check Mailtrap credentials and ensure the SMTP settings are properly configured.
Frontend Not Loading: Ensure ng serve is running and there are no port conflicts on 4200.
