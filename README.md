		# Template_HardwareStore

		## Acquaintance ##
This repository was created as a repository for performing some of the learning tasks of the Udemy course.

		##### Purpose #####
Get practice in development MVC asp.net apps.

		### Step 1 - Introduction. ###
1.	Create solution cmd:\ dotnet new sln
2.	Create ASP.NET MVC project

		### Step 2 - Database setup and Category management. ###

1.	Create new Model Category
2.	Cerate ConnectionStrings in appsetting.json
3.	Add ApplicationContext: Add NuGet-packets SQLServer, EFCore
4.	Add service in startup.cs as services.AddDbContext
5.	Create first Migration: (This method without EF.Tools)
5.1.		- `dotnet tool install --global dotnet-ef` install cmd tool (if not used before)
5.2.		- `dotnet ef migrations add Add_Table_Category` create migration
5.3.		- `dotnet ef database update`
6.	Create Category Controller
7.	Crate Category View Index with bootstrap
8.	Add Category action Create in Controller
9.	Add Category Create View
10.	Add validation:
10.1.		- client as scripts
10.2. 		- server as ModelState in controller and DataAnatation in model
11.	Add Category action Edit in Controller
12.	Add Category Edit View
13.	Add Category action Delete in Controller
14.	Add Category Delete View
