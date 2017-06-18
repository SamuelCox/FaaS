using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FaaS.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload(IFormFile file)
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
