using School.Api.Extensions;
using School.Api.Middlewares;
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

await app.DataSeedAsync();

#region Configure the HTTP request pipeline.
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
