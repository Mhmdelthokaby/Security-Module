using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Core.Entities.Identity;
using Security.Core.Services;
using Security.Repository.Identity;
using Security.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container using Onion Layer Architecture
#region Configure Services

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the DbContext with SQL Server connection string
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();


#endregion

var app = builder.Build();

// Apply pending migrations and create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
        await identityDbContext.Database.MigrateAsync();
        
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        await AppIdentityDbContextSeed.SeedUserAsync(userManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred during the migration");
    }
}

#region Configure

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

#endregion

app.Run();
