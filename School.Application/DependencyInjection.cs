using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace School.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ApplicationAssemblyRef).Assembly);
            });

            services.AddAutoMapper(configAction: config => { },typeof(ApplicationAssemblyRef).Assembly);

            services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyRef).Assembly);

            return services;
        }
    }
}
