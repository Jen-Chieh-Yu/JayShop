using Microsoft.AspNetCore.Http;
using JayShop.DBConnection;
using JayShop.Services;
using JayShop.Models;
using JayShop.Functions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddApplicationServices();

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(10);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// This middleware ensures all URLs are in lowercase.
// It converts the  path of the request to lowercse and redirects to the new lowercase URL.
app.Use(async (context, next) =>
{
    if (context.Request.Method != HttpMethods.Post)
    {
        // Fetch the original path from the request. Example:  "Controller/Action/parameter"
        var originalPath = context.Request.Path.Value;

        // Chech if the original path contains any uppercase letters
        if (originalPath != null && originalPath.Any(char.IsUpper))
        {
            // Covert the entire path to lowercase
            var lowerPath = originalPath.ToLowerInvariant();
            // Update the  request path with the lowercase version
            context.Request.Path = lowerPath;

            // Redirect to the new lowercase URL, preserving the query string
            context.Response.Redirect(lowerPath + context.Request.QueryString, permanent: true);
            return;
        }
    }
    // If no uppercase letters are found, proceed to the next middleware
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Homepage}/{id?}");

app.Run();
