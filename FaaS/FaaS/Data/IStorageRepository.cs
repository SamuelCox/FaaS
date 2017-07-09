using FaaS.Results;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Data
{
    interface IStorageRepository
    {

        Task Persist(string connectionString, string container, string blobName, MemoryStream memoryStream);
        

        Task Delete(string connectionString, string container, string blobName);
        

        Task<BlobResult> Search(string connectionString, string container, string blobName);

        Task<List<IListBlobItem>> ListAll(string connectionString);


    }
}
