using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FaaS.Data;

namespace FaaS.Controllers
{
    public class FileController : Controller
    {
        private IUserManager UserManager { get; }
        private readonly FaaSContext Db;

        public FileController(IUserManager userManager, FaaSContext context)
        {
            UserManager = userManager;
            Db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload(ICollection<IFormFile> files)
        {            
            return View();
        }

        public IActionResult Download(string fileName)
        {

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
