using KuzinShop;
using KuzinShop.Models;
using KuzinShop.Models.DTO;
using KuzinShop.Repositories;
using KuzinShop.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
    })
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("ADMIN"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Время жизни сессии
    options.Cookie.HttpOnly = true;                 // Безопасность Cookie
    options.Cookie.IsEssential = true;              // Необходимость для работы приложения
});

// Добавьте эту строку для регистрации IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Регистрация ProductRepository в контейнере DI
builder.Services.AddScoped<IProductRepository<ProductModel>, ProductRepository>();
builder.Services.AddScoped<IRepository<CategoryModel>, CategoryRepository>();
builder.Services.AddScoped<IAttributeRepository<AttributeModel>, AttributeRepository>();
builder.Services.AddScoped<CategoryAttributesRepository>();
builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<UserRepository>();


var app = builder.Build();

// Автоматическое создание базы данных при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    // Если нужно просто создать базу данных
    dbContext.Database.EnsureCreated();
    // Либо использовать миграции для гибкого управления схемой БД
    // dbContext.Database.Migrate();
}

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

app.UseSession(); // Добавьте этот вызов

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
