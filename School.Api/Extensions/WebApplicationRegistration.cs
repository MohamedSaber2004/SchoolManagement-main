using School.Application.Interfaces;

namespace School.Api.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task DataSeedAsync(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var dataSeedObj = scope.ServiceProvider.GetRequiredService<IDataSeed>();

                await dataSeedObj.IdentityDataSeedAsync();
                await dataSeedObj.DataSeedAsync();
            }
        }
    }
}
