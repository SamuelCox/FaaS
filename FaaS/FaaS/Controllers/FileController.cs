using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FaaS.Data;
using FaaS.Results;

namespace FaaS.Controllers
{
    public class FileController : Controller
    {
        private IUserManager UserManager { get; }
        private readonly FaaSContext Db;
        private readonly AzureStorageRepository repo;

        public FileController(IUserManager userManager, FaaSContext context)
        {
            UserManager = userManager;
            Db = context;
            repo = new AzureStorageRepository();
        }

        public IActionResult Index()
        {
            if (Request.Cookies["LoggedIn"] == "true")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Account", "Index");
            }
            
        }

        public async Task<IActionResult> Upload(ICollection<IFormFile> files, string connectionString, string container)
        {
            foreach(IFormFile file in files)
            {
                MemoryStream stream = new MemoryStream();
                file.CopyTo(stream);
                await repo.Persist(connectionString, container, file.FileName, stream);
            }
            return View();
        }

        public async Task<IActionResult> Download(string connectionString, string container, string fileName)
        {
            BlobResult result = await repo.Search(connectionString, container, fileName);
            return new FileStreamResult(result.Stream, result.ContentType);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
