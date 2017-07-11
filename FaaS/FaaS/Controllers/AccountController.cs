using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FaaS.Data;
using FaaS.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FaaS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly FaaSContext Db;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 FaaSContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Db = context;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public async Task<IActionResult> Login(string userName, string email, string password)
        {
            User user = new User();
            user.UserName = userName;
            user.Email = email;            
            var result = await _signInManager.PasswordSignInAsync(user.Email, password, false, false);            
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "File");
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            
        }

        public async Task<IActionResult> Register(string userName, string email, string password)
        {
            Db.Database.EnsureCreated();
            User user = new User();
            user.UserName = userName;
            user.Email = email;
            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "File");
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        public async Task<IActionResult> Settings()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                
                User user = await _userManager.GetUserAsync(HttpContext.User);                
                List<AzureConnectionString> connections = Db.GetConnectionStrings(user.UserName);

                Settings settings = new Settings();
                settings.Connections = connections;
                return View("Settings", settings);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        public async Task AddConnection(string connection)
        {
            
            AzureConnectionString connectionString = Db.AzureConnectionStrings
                                                        .Where(x => x.ConnectionString == connection).FirstOrDefault();
            if(connectionString == null)
            {
                connectionString = new AzureConnectionString();
                connectionString.ConnectionString = connection;
                Db.AzureConnectionStrings.Add(connectionString);
                Db.SaveChanges();
            }
            User user = await _userManager.GetUserAsync(HttpContext.User);
            

            UserConnection uc = new UserConnection();
            uc.Id = user.Id;
            uc.AzureConnectionStringID = connectionString.AzureConnectionStringID;
            Db.UserConnections.Add(uc);
            Db.SaveChanges();            

        }

        public async Task ModifyConnection(int id, string connectionString)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            AzureConnectionString connection = Db.AzureConnectionStrings.Where(x => x.AzureConnectionStringID == id).FirstOrDefault();
            UserConnection uc = Db.UserConnections.Where(x => x.AzureConnectionStringID == connection.AzureConnectionStringID && x.Id == user.Id).FirstOrDefault();
            if(uc != null)
            {
                AzureConnectionString newConnection = new AzureConnectionString();
                newConnection.ConnectionString = connectionString;
                Db.AzureConnectionStrings.Add(newConnection);
                Db.SaveChanges();

                Db.UserConnections.Remove(uc);
                Db.SaveChanges();

                uc.AzureConnectionStringID = newConnection.AzureConnectionStringID;
                Db.UserConnections.Add(uc);                
                Db.SaveChanges();

            }
        }

        public IActionResult Logout(string userName)
        {
            return View();
        }



        
    }
}