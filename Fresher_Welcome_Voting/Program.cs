var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                      policy.WithOrigins("https://localhost:5002");
                      policy.WithMethods("GET", "POST", "PUT", "DELETE");
                      });
});

// services.AddResponseCaching();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
