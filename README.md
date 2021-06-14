		# Template_HardwareStore

		## Acquaintance ##
This repository was created as a repository for performing some of the learning tasks of the Udemy course.

		##### Purpose #####
Get practice in development MVC asp.net apps.

		### Section 1 - Introduction. ###
1.	Create solution cmd:\ dotnet new sln
2.	Create ASP.NET MVC project

		### Section 2 - Database setup and Category management. ###

1.	Create new Model Category
2.	Cerate ConnectionStrings in appsetting.json
3.	Add ApplicationContext: Add NuGet-packets SQLServer, EFCore
4.	Add service in startup.cs as services.AddDbContext
5.	Create first Migration: (This method without EF.Tools)
5.1.		- `dotnet tool install --global dotnet-ef` install cmd tool (if not used before)
5.2.		- `dotnet ef migrations add Add_Table_Category`- this is create migration
5.3.		- `dotnet ef database update`
6.	Create Category Controller
7.	Crate Category View Index with bootstrap

		### Section 3  ###

1.	Add Category action Create in Controller
2.	Add Category Create View
3.	Add validation:
3.1.		- client as scripts
3.2. 		- server as ModelState in controller and DataAnatation in model
4.	Add Category action Edit in Controller
5.	Add Category Edit View
6.	Add Category action Delete in Controller
7.	Add Category Delete View
	
		### Section 3  ###

1.	Create Model Product
2.	Add DbSet in Context
3.	Create Migration
4.	Update Database
5.	Create Controller Product
6.	Create Index action
7. 	Create Upsert action (it's a bad practice)
8.	
		