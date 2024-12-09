using System;
using System.IO;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;
using JayShop.Models;

namespace JayShop.DBConnection
{
    public class DataBaseConnection : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Product> Products { get; set; }        
        public DbSet<Order> Order { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<District> District { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string server = "127.0.0.1";
            string database = "jayshop";
            string user = "root";
            string password = "jaychiehyu";
            var sslM = MySqlSslMode.Required;
            var version = new MySqlServerVersion(new Version(8, 0, 35));

            string connectString = string.Format($"Server={server};Database={database};UserID={user};Password={password};SslMode={sslM};");
            optionsBuilder.UseMySql(connectString, version);
        }
    }
}
