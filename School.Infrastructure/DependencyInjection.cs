using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Common.Configuration;
using School.Application.Interfaces;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Infrastructure.Data;
using School.Infrastructure.Identity;
using School.Infrastructure.Implementation;
using School.Infrastructure.Implementation.Repositories;
using School.Infrastructure.Implementation.Services;

namespace School.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddDbContext<SchoolDbContext>(options =>
               options.UseSqlServer(_configuration.GetSection("ConnectionStrings")["DbConnection"]));

            services.AddDbContext<SchoolDbContext_Identity>(options =>
               options.UseSqlServer(_configuration.GetSection("ConnectionStrings")["IdentityDbConnection"]));

            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(_configuration.GetSection("ConnectionStrings")["ChatDbConnection"]));

            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<SchoolDbContext_Identity>();

            services.AddSignalR();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChatUnitOfWork, ChatUnitOfWork>();
            services.AddScoped<IDataSeed, DataSeed>();
            services.AddScoped<IServiceManager, ServiceManager>();

            services .AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(serviceProvider => () => serviceProvider.GetRequiredService<IAuthenticationService>());

            services .AddScoped<IStudentService, StudentService>();
            services.AddScoped<Func<IStudentService>>(serviceProvider => () => serviceProvider.GetRequiredService<IStudentService>());

            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<Func<IClassService>>(serviceProvider => () => serviceProvider.GetRequiredService<IClassService>());

            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<Func<ICourseService>>(serviceProvider => () => serviceProvider.GetRequiredService<ICourseService>());

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<Func<IChatService>>(serviceProvider => () => serviceProvider.GetRequiredService<IChatService>());

            services.AddScoped<IChatNotificationService, ChatNotificationService>();
            services.AddScoped<Func<IChatNotificationService>>(serviceProvider => () => serviceProvider.GetRequiredService<IChatNotificationService>());

            services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();
            services.AddScoped<Func<IFirebaseNotificationService>>(serviceProvider => () => serviceProvider.GetRequiredService<IFirebaseNotificationService>());

            services.AddScoped<IMailService, MailService>();
            services.AddScoped<Func<IMailService>>(serviceProvider => () => serviceProvider.GetRequiredService<IMailService>());

            services.Configure<MailSettings>(_configuration.GetSection("MailSettings"));

            return services;
        }
    }
}
