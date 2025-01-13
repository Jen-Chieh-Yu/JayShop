using JayShop.Models;
using JayShop.DBConnection;
using Microsoft.AspNetCore.Mvc;
using JayShop.Services;
using NPOI.OpenXmlFormats.Dml;
using JayShop.ViewModel.User;
using System.Text.RegularExpressions;

namespace JayShop.Functions
{
    public class UserService
    {
        private readonly DataBaseConnection _db;
        private readonly SessionService _sessionService;
        private User? _user;
        private const string SESSION_KEY = "user";
        private const string SESSION_KEY_DATAFORM = "dataForm";

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

        public (bool success, Dictionary<string, string> errors) Login(LoginViewModel loginViewModel)
        {
            var errors = new Dictionary<string, string>();

            if (loginViewModel == null)
            {
                errors["general"] = "";
                return (false, errors);
            }

            if (string.IsNullOrWhiteSpace(loginViewModel.Email) || !IsValidaEmail(loginViewModel.Email))
            {
                errors["email"] = "Email格式不正確";
            }

            if (string.IsNullOrWhiteSpace(loginViewModel.Password) || loginViewModel.Password.Length < 8)
            {
                errors["password"] = "密碼必須包含8個字符";
            }

            if (errors.Any())
            {
                return (false, errors);
            }

            return (true, new Dictionary<string, string>());

            //var user = _db.User.Where(
            //                                                    user=>user.Email==loginViewModel.Email&&
            //                                                    user.Password==loginViewModel.Password)
            //                                                    .Select(user=>user)
            //                                                    .FirstOrDefault();

            //if (user == null)
            //{
            //    errors["user"] = "此帳號尚未註冊";
            //    return (false, errors);
            //}
            //else
            //{
            //    return(true, new Dictionary<string, string>());
            //}

            //var account = loginViewModel.Email;
            //var password = loginViewModel.Password;

            //if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            //{
            //    return false;
            //}
            //else
            //{
            //    var user = _db.User
            //                            .Where(user => user.Email == account && user.Password == password)
            //                            .Select(user => user)
            //                            .FirstOrDefault();
            //    if (user == null)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        _sessionService.SetObjectAsJson(SESSION_KEY, user);
            //        return true;
            //    }
            //}
        }

        public bool RevisePassword(RevisePasswordViewModel revisePasswordViewModel)
        {
            if (revisePasswordViewModel != null)
            {
                var email = revisePasswordViewModel.Email;
                var user = _db.User.Where(user => user.Email == email).FirstOrDefault();
                if (user != null)
                {
                    var newPassword = revisePasswordViewModel.NewPassword;
                    user.Password = newPassword;
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public (bool success, Dictionary<string, string> errors) Register([FromBody] RegisterViewModel registerViewModel)
        {
            var errors = new Dictionary<string, string>();

            if (registerViewModel == null)
            {
                errors["general"] = "註冊資料無效";
                return (false, errors);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(registerViewModel.LastName))
                {
                    errors["lastName"] = "請輸入姓氏";
                }

                if (string.IsNullOrWhiteSpace(registerViewModel.FirstName))
                {
                    errors["firstName"] = "請輸入名字";
                }

                if (string.IsNullOrWhiteSpace(registerViewModel.Email))
                {
                    errors["email"] = "請填寫Email";
                }
                else if (!IsValidaEmail(registerViewModel.Email))
                {
                    errors["email"] = "Email格式不正確";
                }
                //else if (_db.User.Any(user => user.Email == registerViewModel.Email))
                //{
                //    errors["email"] = "此Email已註冊過";
                //}

                if (string.IsNullOrWhiteSpace(
                                                                        registerViewModel.Password) ||
                                                                        registerViewModel.Password.Length < 8)
                {
                    errors["password"] = "密碼必須包含8個字符";
                }
                if (string.IsNullOrWhiteSpace(
                                                                        registerViewModel.ConfirmPassword) ||
                                                                        registerViewModel.ConfirmPassword.Length < 8 ||
                                                                        registerViewModel.Password != registerViewModel.ConfirmPassword)
                {
                    errors["confirmPassword"] = "密碼與確認密碼不一致，請重新輸入";
                }

                if (string.IsNullOrWhiteSpace(registerViewModel.PhoneNumber))
                {
                    errors["phoneNumber"] = "請填寫手機號碼";
                }
                else if (!IsVaildaPhoneNumber(registerViewModel.PhoneNumber))
                {
                    errors["phoneNumber"] = "手機號碼格是不正確";
                }

                if (registerViewModel.Year == 0)
                {
                    errors["year"] = "請選擇年份";
                }

                if (registerViewModel.Month == 0)
                {
                    errors["month"] = "請選擇月份";
                }

                if (registerViewModel.Day == 0)
                {
                    errors["day"] = "請選擇日期";
                }

                if (errors.Any())
                {
                    return (false, errors);
                }

                var user = new User()
                {
                    UserName = registerViewModel.LastName + registerViewModel.FirstName,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    Phone = registerViewModel.PhoneNumber,
                    BirthDay = new DateTime(
                                                                            registerViewModel.Year,
                                                                            registerViewModel.Month,
                                                                            registerViewModel.Day
                                                                                                                        ).Date
                };
                //_db.Add(user);
                //_db.SaveChanges();
                return (true, new Dictionary<string, string>());
            }



            //if (registerViewModel != null)
            //{
            //    var email = registerViewModel.Email;
            //    var existUser = _db.User
            //                .Where(user => user.Email == email)
            //                .Select(user => user);

            //    if (existUser == null)
            //    {
            //        var user = new User()
            //        {
            //            UserName = registerViewModel.LastName + registerViewModel.FirstName,
            //            Email = registerViewModel.Email,
            //            Password = registerViewModel.Password,
            //            Phone = registerViewModel.PhoneNumber,
            //            BirthDay = new DateTime(
            //                                                                registerViewModel.Year,
            //                                                                registerViewModel.Month,
            //                                                                registerViewModel.Day
            //                                                                                                            ).Date
            //        };
            //_db.User.Add(user);
            //_db.SaveChanges();
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //return false;
        }

        public void SaveRegisterForm(RegisterViewModel registerViewModel)
        {
            if (registerViewModel != null)
            {
                registerViewModel.Password = "";
                registerViewModel.ConfirmPassword = "";
                _sessionService.SetObjectAsJson(SESSION_KEY_DATAFORM, registerViewModel);
            }
            else
            {
                Console.WriteLine("The register form is something wrong.");
            }
        }

        public RegisterViewModel? GetFormData()
        {
            var registerFormData = _sessionService.GetObjectFromJson<RegisterViewModel>(SESSION_KEY_DATAFORM);

            return registerFormData;
        }

        private bool IsValidaEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, emailRegex);
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            bool hasLetter = false;
            bool hasDigit = false;

            foreach(char character in password)
            {
                if (char.IsLetter(character))
                {
                    hasLetter = true;
                }
                else if (char.IsDigit(character)) 
                {
                    hasDigit = true;
                }

                if (hasLetter && hasDigit)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsVaildaPhoneNumber(string phoneNumber)
        {
            var phoneRegex = @"^09\d{8}$";

            return Regex.IsMatch(phoneNumber, phoneRegex);
        }

        public void Logout()
        {
            _sessionService.RemoveSession(SESSION_KEY);
        }
    }
}
