namespace FitnessBuddy.Web.Infrastructure.Extensions
{
    using CloudinaryDotNet;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data;
    using FitnessBuddy.Data.Common;
    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        public static IServiceCollection ConfigureCookiePolicyOptions(this IServiceCollection services)
            => services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

        public static IServiceCollection AddControllersWithAutoAntiforgeryTokenAttribute(this IServiceCollection services)
        {
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();

            return services;
        }

        public static IServiceCollection AddFacebookAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = configuration["Facebook:AppId"];
                    options.AppSecret = configuration["Facebook:AppSecret"];
                });

            return services;
        }

        public static IServiceCollection AddGoogleAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Google:ClientId"];
                    options.ClientSecret = configuration["Google:ClientSecret"];
                });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            return services;
        }

        public static IServiceCollection AddAntiforgeryHeader(this IServiceCollection services)
          => services
            .AddAntiforgery(options => options.HeaderName = GlobalConstants.AntiforgeryHeaderName);

        public static IServiceCollection AddCloudinaryConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(CloudinaryConfiguration(configuration));

            return services;
        }

        private static Cloudinary CloudinaryConfiguration(IConfiguration configuration)
        {
            var cloudinaryCredentials = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]);

            return new Cloudinary(cloudinaryCredentials);
        }
    }
}
