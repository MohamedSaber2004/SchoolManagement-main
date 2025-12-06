using Microsoft.EntityFrameworkCore;
using School.Api.Extensions;
using School.Api.Middlewares;
using School.Infrastructure.Data;
using School.Infrastructure.Implementation.Hubs;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiEndPointServices();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddWebApiValidationServices();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
#endregion

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<SchoolDbContext>();
    await dbContext.Database.MigrateAsync();

    var identityContext = services.GetRequiredService<SchoolDbContext_Identity>();
    await identityContext.Database.MigrateAsync();

    var chatContext = services.GetRequiredService<ChatDbContext>();
    await chatContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    await app.DataSeedAsync();
}

#region Configure the HTTP request pipeline.
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AngularAppPolicy");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chathub");
app.MapControllers();
#endregion

app.Run();
