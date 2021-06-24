# Template_HardwareStore

## Acquaintance
This repository was created as a repository for performing some of the learning tasks of the Udemy course.

			##### Purpose
Get practice in development MVC asp.net apps.

			### Section 1 - Introduction.
1.		Create solution cmd:\ dotnet new sln
2.		Create ASP.NET MVC project

			### Section 2 - Database setup and Category management.

1.		Create new Model Category
2.		Cerate ConnectionStrings in appsetting.json
3.		Add ApplicationContext: Add NuGet-packets SQLServer, EFCore
4.		Add service in startup.cs as services.AddDbContext
5.		Create first Migration: (This method without EF.Tools)
5.1.		- `dotnet tool install --global dotnet-ef` install cmd tool (if not used before)
5.2.		- `dotnet ef migrations add Add_Table_Category`- this is create migration
5.3.		- `dotnet ef database update`
6.		Create Category Controller
7.		Crate Category View Index with bootstrap

			### Section 3

1.		Add Category action Create in Controller
2.		Add Category Create View
3.		Add validation:
3.1.		- client as scripts
3.2. 		- server as ModelState in controller and DataAnatation in model
4.		Add Category action Edit in Controller
5.		Add Category Edit View
6.		Add Category action Delete in Controller
7.		Add Category Delete View
	
			### Section 4

1.		Create Model Product
2.		Add DbSet in Context
3.		Create Migration
4.		Update Database
5.		Create Controller Product
6.		Create Index action
7. 		Create Upsert action (it's a bad practice)
8.		Create ViewModel/ and use there split model
9.  	Create Constants/WebConstants.cs
10.		In Upsert action create two action for creating and update
11.		Create action Delete
12. 	Create view Delete

			### Section 5 (Include settings of UI)
		
1.		Change Nav bashboard
2.		Make new Home Index page
3.		Create pasrtialView card of product
4.		Create filters by JS
5.		Create get action Details   
6.  	Create "Basket for Product" implement this with Sessions:
6.1. 		- add services in startup.cs: AddHttpContextAccessor, AddSession
6.2.		- add session in application pinpline
7.		Add new model ShoppingCart
8. 		Set new variable in WebConstants
9. 		Use FontAverson and part of functions our Session
10. 	Add functions session in Home controller action Details
11. 	Add action RremoveFromCart to Remove cart from list

			### Section 6 (Setting Identity system)
		
1.		Add NugetPacket Microsoft Identity Core
2.		Add inheritance IdentityDbContext
3.		Add Microsoft.AspNetCore.Identity.UI   
4.		Set settings in startup.sc
5.		Create new migration
6.  	Update Database
7.		Add new scaffolded Item as Identity 
8.		Create auto razerpages to register & login
9. 		To plug _IdentityPartialview to Layout
10. 	Add new model ApplicationUser and inheritance from IdentityUser
11.		Create new migration
12.		Update Database
13.		Change Razor pages register.cshtml IdentityUser to ApplicationUser
14. 	Add UserRole in register.cshtml and set this in GET request
15. 	Add UserRole in DI container
16.		Set logic to create Admin only Admin
17.		Change UI RolePages

			### Section 5 (Include settings of UI)
		
Название файла  | Содержание файла
----------------|----------------------
style.css       | Пустой файл каскадной таблицы стилей, в который производится сбока необходимых стилей
reset.css       | Reset CSS от Эрика Мейера
normalize.css   | Нормалайзер CSS от Nicolas Gallagher
block.css       | Основные стили блоков системы
addition.css    | Дополнительные стили
fontawesome.css | Стили иконочного шрифта
layout.css      | Основные стили, применительно к определённому сайту
lightbox.css    | Стили лайтбокса, если таковой используется
index.html      | Индексный файл для проверки вносимых изменений
