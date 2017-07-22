using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FaaS.Data;
using FaaS.Results;
using Microsoft.AspNetCore.Identity;
using FaaS.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Caching.Memory;

namespace FaaS.Controllers
{
    public class FileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly FaaSContext Db;
        private readonly AzureStorageRepository repo;
        private readonly IMemoryCache _cache;

        public FileController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 FaaSContext context,
                                 IMemoryCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Db = context;
            repo = new AzureStorageRepository();
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                List<FileIndexModel> cachedModels = null;
                if (_cache.TryGetValue<List<FileIndexModel>>(HttpContext.User.ToString() + "_files", out cachedModels))
                {
                    return View(cachedModels);
                }
                else
                {

                    List<FileIndexModel> models = new List<FileIndexModel>();
                    User user = await _userManager.GetUserAsync(HttpContext.User);
                    List<AzureConnectionString> connections = Db.GetConnectionStrings(user.UserName);
                    foreach (AzureConnectionString connection in connections)
                    {
                        FileIndexModel model = new FileIndexModel();
                        model.ConnectionString = connection.ConnectionString;
                        List<IListBlobItem> blobs = await repo.ListAll(connection.ConnectionString);

                        foreach (CloudBlob blob in blobs)
                        {
                            model.AddItem(blob.Container.Name, blob);
                        }
                        models.Add(model);
                    }
                    _cache.Set<List<FileIndexModel>>(HttpContext.User.ToString() + "_files", models);
                    return View(models);
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            
            
        }

        public async Task<IActionResult> Upload(string container, ICollection<IFormFile> files, string connectionString)
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                _cache.Remove(HttpContext.User.ToString() + "_files");
                foreach (IFormFile file in files)
                {
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    _cache.Remove(HttpContext.User.ToString() + connectionString + "_" + container + "_" + file.FileName);
                    await repo.Persist(connectionString, container, file.FileName, stream);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
                                                            
        }


        public async Task<IActionResult> Download(string connectionString, string container, string fileName)
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                BlobResult cachedResult = null;
                if(_cache.TryGetValue<BlobResult>(HttpContext.User.ToString() + connectionString + "_" + container + "_" + fileName, out cachedResult))
                {
                    return new FileStreamResult(cachedResult.Stream, cachedResult.ContentType);
                }
                BlobResult result = await repo.Search(connectionString, container, fileName);
                _cache.Set<BlobResult>(HttpContext.User.ToString() + connectionString + "_" + container + "_" + fileName, result);
                return new FileStreamResult(result.Stream, result.ContentType);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            
            
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
