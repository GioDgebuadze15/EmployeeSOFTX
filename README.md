# EmployeeSoftX

# Getting Started. 
The following instructions will help you setup the project on your local development machine.
### Prerequisites
	1. ASP.NET Core - The main c# based server side platform. Version .Net 6.
	2. MSSQL - You will ether need to download and install the Microsoft SQL server.
	
### Installing
All we are trying to achieve is getting the code on to your computer and making sure the project is pointing to your database and that the database is created.

	1. After setting up the prerequisites, git clone or fork this repository.
	2. Set the DefaultDb connection string in the appsettings.json file in Employee.Api, to connect to your database in this project.
	3. Add migrations to create database using enity framework.
	4. run KeyGenerator and move key file into Employee.Api and Employee.Mvc.
