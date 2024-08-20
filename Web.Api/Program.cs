using Microsoft.EntityFrameworkCore;
using Web.Api.Database;
using Web.Api.Users;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.CustomSchemaIds(id => id.FullName!.Replace('+', '-')));

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("db"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<LoginUser>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

UserEndpoints.Map(app);

app.Run();
