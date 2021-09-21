# Template_HardwareStore

## Acquaintance
This repository was created as a repository for performing some of the learning tasks of the Udemy course.

##### Purpose
Get practice in development MVC asp.net apps.

#### ChAPTER 1 

### Section 1.1 - Introduction.
1. Create solution cmd:\ dotnet new sln
2. Create ASP.NET MVC project

### Section 1.2 - Database setup and Category management.

1. Create new Model Category
2. Create ConnectionStrings in appsetting.json
3. Add ApplicationContext: Add NuGet-packets SQLServer, EFCore
4. Add service in startup.cs as services.AddDbContext
5. Create first Migration: (This method without EF.Tools)
5.1. - `dotnet tool install --global dotnet-ef` install cmd tool (if not used before)
5.2. - `dotnet ef migrations add Add_Table_Category`- this is create migration
5.3. - `dotnet ef database update`
6. Create Category Controller
7. Crate Category View Index with bootstrap

### Section 1.3

1. Add Category action Create in Controller
2. Add Category Create View
3. Add validation:
3.1. - client as scripts
3.2. - server as ModelState in controller and DataAnatation in model
4. Add Category action Edit in Controller
5. Add Category Edit View
6. Add Category action Delete in Controller
7. Add Category Delete View
	
### Section 1.4

1. Create Model Product
2. Add DbSet in Context
3. Create Migration
4. Update Database
5. Create Controller Product
6. Create Index action
7. Create Upsert action (it's a bad practice)
8. Create ViewModel/ and use there split model
9. Create Constants/WebConstants.cs
10. In Upsert action create two action for creating and update
11. Create action Delete
12. Create view Delete

### Section 1.5 (Include settings of UI)
		
1. Change Nav bashboard
2. Make new Home Index page
3. Create pasrtialView card of product
4. Create filters by JS
5. Create get action Details   
6. Create "Basket for Product" implement this with Sessions:
6.1. - add services in startup.cs: AddHttpContextAccessor, AddSession
6.2. - add session in application pinpline
7. Add new model ShoppingCart
8. Set new variable in WebConstants
9. Use FontAverson and part of functions our Session
10. Add functions session in Home controller action Details
11. Add action RremoveFromCart to Remove cart from list

### Section 1.6 (Setting Identity system)
		
1. Add NugetPacket Microsoft Identity Core
2. Add inheritance IdentityDbContext
3. Add Microsoft.AspNetCore.Identity.UI   
4. Set settings in startup.sc
5. Create new migration
6. Update Database
7. Add new scaffolded Item as Identity 
8. Create auto razerpages to register & login
9. To plug _IdentityPartialview to Layout
10. Add new model ApplicationUser and inheritance from IdentityUser
11. Create new migration
12. Update Database
13. Change Razor pages register.cshtml IdentityUser to ApplicationUser
14. Add UserRole in register.cshtml and set this in GET request
15. Add UserRole in DI container
16. Set logic to create Admin only Admin
17. Change UI RolePages

### Section 1.7 

1. Add controller for basket (CartController)
2. Add view Cart
3. CartController - Autorise
4. Create ProductUserViewModel
5. Create Action IndexPost and Summary
6. Create view Summory
7. Register in Mailjet.Api (Be attansion! I used old worker version 1.2.2)
8. Create Utility/EmailSender/
9. Try to send email
10. Use Configuration by appsettings.json
11. Add wwwroot/templates/Inquary.html
12. Create action SummaryPost
13. Create SummaryInquary view 
14. Use in Summary view `for` instead of the `foreach` and hidden item.Id and item.name
15.	Render Inquiry.html and insert item
16. Add send logic

#### CHAPTER 2 

### Section 2.2 - Changing the architecture of the project.

9. Introduction
10. - 11. Move services in it's own project `Utility`
12. Move all Models in it's own project `Entities`
13. - 14. Create DAL layer
15. Create test migration 
16. Add plagins: 
				- new version of [bootstrap](https://getbootstrap.com/)
				- [CodeSeven/toastr](https://github.com/CodeSeven/toastr)
				- [Syncfusion](https://ej2.syncfusion.com/aspnetcore/Grid/GridOverview#/material)
				- [Datatables](https://datatables.net/)
17. - 18. Setup settings of bootstrap.

### Section 2.3 - Repository.

20. - 22. Make Interface Patern Repository
23. Create separate Repository for Category
24. Change in Category controller context to repository
25. Create separate Repository for ApplicationType and Change in ApplicationType controller context to repository
26. Create separate Repository for Products
27. Change in Products controller context to repository

### Section 2.4 

29. Crate InquiryHeader model
30. Create InquiryDetail model
31. Use changes into database
32. Insert this model in Repository
33. - 34. Modify controller and repair _db 
35. Save responses in CartController InquiryDetail and InquiryHeader
36. Create InquiryControler 

### Section 2.5

37. Create Inquiry Index view
38. Add JS for table
39. Add action Details in InquaryController
40. Add Inquiry detail View
41. Setting view details
42. Setting function add Inquary detail to basket
43. Create RemoveRange in Repository and action Delete in InquaryComtroller
