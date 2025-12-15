 Secure RBAC & Audit API

This is a small ASP.NET Core Web API project focused on backend security basics.

The main goal of the project is to demonstrate how authentication, authorization,
and auditing can be handled in a clean and simple way.

 What this project does
- Uses JWT for authentication
- Applies role-based access control (Admin / Operator / Viewer)
- Logs sensitive actions for audit purposes
- Includes basic protection against repeated failed login attempts
- Exposes secured endpoints through Swagger

 Example roles
 User       Role      
-----------------
 admin      Admin     
 operator   Operator  
 viewer     Viewer    

 Why this project
I built this project to better understand how secure backend systems are structured,
especially in environments where access control and traceability are important
(e.g. enterprise or defense-related systems).

 Tech stack
- ASP.NET Core (.NET 8)
- JWT Authentication
- Swagger / OpenAPI

 Notes
This project uses an in-memory data store for simplicity.
It is intended as a learning and demonstration project rather than a production-ready system.
