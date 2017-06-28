using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FaaS.Data;
using FaaS.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FaaS.Controllers
{
    public class AccountController : Controller
    {
        private IUserManager UserManager { get; }
        private readonly FaaSContext Db;

        public AccountController(IUserManager userManager, FaaSContext context)
        {
            UserManager = userManager;
            Db = context;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Login(string userName, string password)
        {
            
            User user = Db.Users.Where(x => x.UserName == userName).FirstOrDefault();
            byte[] salt = Convert.FromBase64String(user.Salt);
            byte[] key = Convert.FromBase64String(user.PasswordHash);
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);
            if(hash.SequenceEqual(key))
            {
                Response.Cookies.Append("LoggedIn", "true");
                Response.Cookies.Append("User", userName);
                return RedirectToAction("Index", "File");
            }
            else
            {
                return View();
            }


            

            
        }

        public IActionResult Register(string userName, string password)
        {
            Db.Database.EnsureCreated();
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[128 / 8];
                rng.GetBytes(salt);

                byte[] key = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);

                string keyHexString = Convert.ToBase64String(key);
                
                User user = new User();
                user.UserName = userName;
                user.PasswordHash = keyHexString;
                user.Salt = Convert.ToBase64String(salt);
                Db.Users.Add(user);
                Db.SaveChanges();
                
            }
            Response.Cookies.Append("LoggedIn", "true");
            Response.Cookies.Append("User", userName);
            return RedirectToAction("Index", "File");
        }

        public IActionResult Logout(string userName)
        {
            return View();
        }



        
    }
}