namespace FitnessBuddy.Web
{
    using System.Reflection;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data;
    using FitnessBuddy.Data.Common;
    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Repositories;
    using FitnessBuddy.Data.Seeding;
    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Services.Data.ArticlesRatings;
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.ExercisesLikes;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.MealsFoodsService;
    using FitnessBuddy.Services.Data.Messages;
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Services.Data.Replies;
    using FitnessBuddy.Services.Data.Trainings;
    using FitnessBuddy.Services.Data.TrainingsExercises;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Data.UsersFollowers;
    using FitnessBuddy.Services.Hubs;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Services.Messaging;
    using FitnessBuddy.Web.Hubs;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });
            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();

            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = this.configuration.GetValue<string>("Facebook:AppId");
                    options.AppSecret = this.configuration.GetValue<string>("Facebook:AppSecret");
                })
                .AddGoogle(options =>
                {
                    options.ClientId = this.configuration.GetValue<string>("Google:ClientId");
                    options.ClientSecret = this.configuration.GetValue<string>("Google:ClientSecret");
                });

            services.AddRazorPages();
            services.AddSignalR();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options => options.HeaderName = GlobalConstants.AntiforgeryHeaderName);

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IFoodNamesService, FoodNamesService>();
            services.AddTransient<IFoodsService, FoodsService>();
            services.AddTransient<IMealsService, MealsService>();
            services.AddTransient<IMealsFoodsService, MealsFoodsService>();
            services.AddTransient<IExercisesService, ExercisesService>();
            services.AddTransient<IExerciseCategoriesService, ExerciseCategoriesService>();
            services.AddTransient<IExerciseEquipmentService, ExerciseEquipmentService>();
            services.AddTransient<ITrainingsService, TrainingsService>();
            services.AddTransient<ITrainingsExercisesService, TrainingsExercisesService>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<IArticleCategoriesService, ArticleCategoriesService>();
            services.AddTransient<IPostCategoriesService, PostCategoriesService>();
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<IRepliesService, RepliesService>();
            services.AddTransient<IUsersFollowersService, UsersFollowersService>();
            services.AddTransient<IExercisesLikesService, ExercisesLikesService>();
            services.AddTransient<IArticlesRatingsService, ArticlesRatingsService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<IGroupNameProvider, GroupNameProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                },
            });
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/chat");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
