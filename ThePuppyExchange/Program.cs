using BusinessLogicLayer;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load("../.env");

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adds DB Access for puppy information in remote postgresql database
builder.Services.AddDbContext<PuppyDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPuppyRepository, PuppyRepository>();
builder.Services.AddScoped<IPuppyService, PuppyService>();

// Adds DB Access for customer information in remote postgresql database
builder.Services.AddDbContext<CustomerDBContext>(options => options.UseNpgsql(connectionString));


// Adds DB Access for user privilege information in remote postgresql database
builder.Services.AddDbContext<PrivilegeDBContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Allows for session storage of user information for login/logout functionality
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enables session storage for user information
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Home}/{id?}");

app.Run();
