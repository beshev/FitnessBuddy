# FitnessBuddy

## 🔗 **Link to the project**
&nbsp;&nbsp;&nbsp;&nbsp;**[fitness-buddy.azurewebsites.net/](https://fitness-buddy.azurewebsites.net/)**
<br/><br/>

## :eyeglasses: Project Introduction
<p>Fitness application built for <a href="https://softuni.bg/trainings/3601/asp-dot-net-core-february-2022">ASP.NET Core course</a> at SoftUni</p>

## 📝 Project Overview
-	**FitnessBuddy** is a ready to use application for fitness and nutrition which is easy to use and has a simple user-friendly interface. The backend is developed with MS SQL and Entity Framework Core, and the frontend is built with Bootstrap and Java Script.
-	The application requires a simple registration process.
-	Users can create and search for foods and exercises. Only the administrator and creator can delete foods and exercises in case they consist of inappropriate content.
-	Users can generate eating and workout plans (diaries) which can be edited and deleted.
-	Users can create, edit, delete, search and see details about forum posts, forum post comments and blog articles. Before deletion a pop up window asks for confirmation
-	Users can follow and unfollow another users. They can also send Emails to another users thanks to the integrated SendGrid client.
-	The administrator has a special Admin Area from where he/she can create, update, delete and see details about users, foods, exercises, articles, forum posts and post replies. The administrator can also ban and unban users. A banned user cannot access the full functionallity of the application.
-	The application has preloaded (seeded) user roles, users (admin and user), foods, exercises, exercise categories, exercise equipment, article categories and forum categories.
<br/><br/>

## 🧪 Test Accounts
&nbsp;&nbsp;&nbsp;&nbsp;Email: **admin@admin.com**  
&nbsp;&nbsp;&nbsp;&nbsp;Password: **123456**  

&nbsp;&nbsp;&nbsp;&nbsp;Email: **user@user.com**  
&nbsp;&nbsp;&nbsp;&nbsp;Password: **123456** 
<br/><br/>

## :hammer: Built with:
* [ASP.NET 6.0](https://github.com/dotnet/aspnetcore)
* [Visual Studio 2022](https://github.com/github/VisualStudio)
* [Entity Framework Core 6.0](https://github.com/dotnet/efcore)
* [Sql Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [SendGrid](https://github.com/sendgrid)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [Bootstrap](https://github.com/twbs/bootstrap)
* [Font Awesome](https://fontawesome.com/)
* [xUnit](https://github.com/xunit/xunit)
* [Moq](https://github.com/moq/moq)
* [MockQueryable](https://github.com/romantitov/MockQueryable)
* [FluentAssertions](https://github.com/fluentassertions/fluentassertions)
* [TinyMCE](https://github.com/tinymce)
* [SignalR](https://github.com/SignalR/SignalR)
* [CloudinaryDotNet](https://github.com/cloudinary/CloudinaryDotNet)
* [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer)
<br/><br/>

## :pencil2: Code quality 
- **Using of**
   * **ASP.NET Areas**
   * **StyleCop**
   * **Repository Pattern**
   * **Dependency Injection**
   * **IFormFle** for uploading of JPG images as well as an option for adding links of images and YouTube videos.
   * **Method** for **embedding of raw YouTube links**.
   * **AuthorizationFilterAttribute** to restrict the access to the full functionality of the application for logged-in but banned users.
   * Separate storage for **data constants**.
<br/><br/>

## :handshake: Credits
- Using [ASP.NET Core Template](https://github.com/NikolayIT/ASP.NET-Core-Template) originally developed by:
   * [Nikolay Kostov](https://github.com/NikolayIT)
   * [Vladislav Karamfilov](https://github.com/vladislav-karamfilov)
   * [Stoyan Shopov](https://github.com/StoyanShopov)
<br/><br/>

## :v: Leave a feedback
Give a :star: if you like it.
Thank you ❤️
<br/><br/>

## 📖 License
See the [LICENSE](https://github.com/beshev/FitnessBuddy/blob/main/LICENSE) file for details
