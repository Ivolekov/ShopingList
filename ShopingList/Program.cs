using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopingList.Data;
using ShopingList.Data.Models;
using ShopingList.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShopingListDBContextConnection") ?? throw new InvalidOperationException("Connection string 'ShopingListDBContextConnection' not found.");

builder.Services.AddDbContext<ShopingListDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ShopingListDBContext>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
    .AddScoped<ICategoryService, ProductService>()
    .AddScoped<IProductService, ProductService>()
    .AddControllers();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Index}"
    );
});

app.Run();
