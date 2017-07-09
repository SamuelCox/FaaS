﻿using System;
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

namespace FaaS.Controllers
{
    public class FileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly FaaSContext Db;
        private readonly AzureStorageRepository repo;

        public FileController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 FaaSContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Db = context;
            repo = new AzureStorageRepository();
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            
            
        }

        public async Task<IActionResult> Upload(ICollection<IFormFile> files, string connectionString, string container)
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                foreach (IFormFile file in files)
                {
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
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
                BlobResult result = await repo.Search(connectionString, container, fileName);
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
