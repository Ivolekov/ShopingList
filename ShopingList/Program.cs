using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopingList.Data;
using ShopingList.Data.Models;
using ShopingList.Features.Products;
using ShopingList.Features.Products.Services;
using ShopingList.Features.ShopingLists;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShopingListDBContextConnection") ?? throw new InvalidOperationException("Connection string 'ShopingListDBContextConnection' not found.");

builder.Services.AddDbContext<ShopingListDBContext>(options => {
    options.UseNpgsql(connectionString);
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<ShopingListDBContext>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
    .AddScoped<ICategoryService, ProductService>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IShopingListService, ShopingListService>()
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
});

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<ProductsController>>();
builder.Services.AddSingleton(typeof(ILogger), logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}"
    );
});
app.Seed();
app.Run();
