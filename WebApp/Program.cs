using BLL.Services;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Environment.IsDevelopment() 
    ? builder.Configuration.GetConnectionString("LocalConnection")
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

//API
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var apiBaseAddress = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5052";

builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri($"{apiBaseAddress}/");
});

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IMoneyService, MoneyService>();
builder.Services.AddScoped<IProductReceiptService, ProductReceiptService>();

// Import product config services
builder.Services.AddScoped<IProductConfigurationService, ProductConfigurationService>();
builder.Services.AddScoped<IProductImportService, ProductImportService>();

// Authorisation session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Bake Sale API", 
        Version = "v1",
        Description = "API for managing bake sale products and transactions"
        
    });
    c.EnableAnnotations();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DAL.Seeding.DbInitializer.Initialize(services);
}

// Swagger konfig
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bake Sale API v1"));
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.UseSession();

app.Run();