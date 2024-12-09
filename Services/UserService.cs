using JayShop.Models;
using JayShop.DBConnection;
using Microsoft.AspNetCore.Mvc;
using JayShop.Services;
using NPOI.OpenXmlFormats.Dml;

namespace JayShop.Functions
{
    public class UserService
    {
        private readonly DataBaseConnection _db;
        private readonly SessionService _sessionService;
        private User? _user;
        private const string SESSION_KEY = "user";

        public UserService(DataBaseConnection db, SessionService sessionService)
        {
            _db = db;
            _sessionService = sessionService;
        }

        public User? GetUser()
        {
            _user = _sessionService.GetObjectFromJson<User>(SESSION_KEY);

            return _user;
        }

        public bool Login(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            else
            {
                var user = _db.User
                                        .Where(user => user.Email == account && user.Password == password)
                                        .Select(user => user)
                                        .FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    _sessionService.SetObjectAsJson(SESSION_KEY, user);
                    return true;
                }
            }
        }

        public bool RevisePassword(string newPassword)
        {
            if (_user == null || string.IsNullOrEmpty(newPassword))
            {
                return false;
            }
            else
            {
                _user.Password = newPassword;
                _db.SaveChanges();

                return true;
            }
        }

        public bool Register(User user)
        {
            var existUser = _db.User
                                        .Where(user => user.Email == user.Email)
                                        .Select(user=>user);

            if (existUser == null || user != null)
            {
                _db.User.Add(user);
                _db.SaveChanges();

                return true;
            }
            else
            {                
                return false;
            }
        }

        public void Logout()
        {
            _sessionService.RemoveSession(SESSION_KEY);
        }
    }
}
