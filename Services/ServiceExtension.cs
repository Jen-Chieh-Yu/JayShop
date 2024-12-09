using JayShop.DBConnection;
using JayShop.Functions;

namespace JayShop.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<DataBaseConnection>();
            services.AddScoped<SessionService>();
            services.AddScoped<SearchService>();
            services.AddScoped<UserService>();
            services.AddScoped<ProductService>();
            services.AddScoped<CartService>();
            services.AddScoped<OrderService>();
            services.AddScoped<AddressService>();

            return services;
        }
    }
}
